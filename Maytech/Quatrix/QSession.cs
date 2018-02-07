using Maytech.Quatrix.Operations;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace Maytech.Quatrix {

    [Serializable]
    public class QSession : QRequest, IDeserializationCallback, IDisposable {

        protected const string AUTH_TYPE = "mfa-code";
        protected internal const string MFA_TYPE = "2FA";
        protected internal const string USER_PIN_TYPE = "PIN";

        //User data
        private TimeSpan keepAliveLoopTime = TimeSpan.FromSeconds(900.0);
        private string email = string.Empty;
        private string password = string.Empty;

        [field: NonSerialized]
        private SessionKeepAlive sessionKeepAlive;

        /// <summary>
        /// Occurs after successfully login
        /// </summary>
        [field: NonSerialized]
        public event EventHandler LoginedEvent;

        /// <summary>
        /// Occurs after logout
        /// </summary>
        [field: NonSerialized]
        public event EventHandler LogoutedEvent;

        /// <summary>
        /// Gets or sets saving user data options (Clean up after success login)
        /// </summary>
        public RememberOptions Remember { get; set; }

        public SessionStatus Status { get; private set; }

        public Session Session { get; private set; }

        /// <summary>
        /// Gets information about user profile
        /// </summary>
        public Profile Profile { get; private set; }

        public Entity.AccountInfo AccountInfo { get; private set; }

        /// <summary>
        /// Gets Quatrix host name
        /// </summary>
        public string Host {
            get {
                return this.Referer;
            }
        }

        /// <summary>
        /// Gets or sets email used to log in Quatrix account
        /// </summary>
        public string Email {
            get {
                return email;
            }
            set {
                Validation.Email(value);
                email = value.ToLower();
            }
        }

        /// <summary>
        /// Gets or sets users password
        /// </summary>
        public string Password {
            get {
                return password;
            }
            set {
                Validation.Password(value);
                password = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the session will be refreshed after some time
        /// </summary>
        public bool KeepAlive {
            get {
                return sessionKeepAlive.KeepAlive;
            }
            set {
                sessionKeepAlive.KeepAlive = value;
            }
        }

        /// <summary>
        /// Set this when <seealso cref="MultiFactorAuthentication"/> occurs
        /// </summary>
        public string AuthCode { get; set; }

        public override string Token {
            get {
                return Session.session_id;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QSession"/> class with the specified host name.
        /// </summary>
        /// <remarks>All parameters are validating  </remarks>
        /// <param name="host">Quatrix name where account is hosted</param>
        public QSession(string host) : base(host) {
            AccountInfo = AccountActions.AccountInfo(this);
            if(AccountInfo.IsSuspended) {
                throw new QuatrixNotAllowedException();
            }
            Session = new Session();
            Remember = RememberOptions.HostLogin;
            sessionKeepAlive = new SessionKeepAlive(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QSession"/> class with the specified host name and email.
        /// </summary>
        /// <remarks>All parameters are validating  </remarks>
        /// <param name="host">Quatrix name where account is hosted</param>
        /// <param name="email">Email, used to log in Quatrix account</param>
        public QSession(string host, string email)
            : this(host) {
            this.Email = email;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QSession"/> class with the specified hostname, email and password.
        /// </summary>
        /// <remarks>All parameters are validating  </remarks>
        /// <param name="host">Quatrix name where account is hosted </param>
        /// <param name="email">Email, used to log in Quatrix account</param>
        /// <param name="password">Users password</param>
        public QSession(string host, string email, string password)
            : this(host, email) {
            Password = password;
        }

        /// <summary>
        /// Used to login in Quatrix account (this method create new seesion)
        /// </summary>
        /// <exception cref="QuatrixAuthorizationException"></exception>
        /// <returns>True if user is successfully logged in. Otherwise - false.</returns>
        public bool Login() {
            return this.Login(false);
        }

        /// <summary>
        /// Used to login in Quatrix account (this method create new seesion)
        /// </summary>
        /// <param name="newSession">Create new session</param>
        /// <exception cref="QuatrixAuthorizationException"></exception>
        /// <returns>True if user is successfully logged in. Otherwise - false.</returns>
        internal virtual bool Login(bool newSession) {
            if(newSession) {
                RemoveToken();
            }
            //return true if user are already logged in
            else if(Status == SessionStatus.LoggedIn) {
                return true;
            }
            //create new session
            if(Status <= SessionStatus.PartialLoggedIn) {
                HttpResult res = OnUserLogin();
                if(res.Code == STATUS_MULTI_STATUS) {
                    //partial logined
                    //detect auth type
                    string value = res.Headers[AUTH_TYPE];
                    if(string.IsNullOrEmpty(value)) {
                        return false;
                    }
                    int code = int.Parse(value);
                    if(code == QuatrixException.NOT_ALLOWED_WITHOUT_2FA_ERROR) {
                        //handle authrizaion code
                        Status = SessionStatus.AuthorizationCodeRequired;
                    } else if(code == QuatrixException.NOT_ALLOWED_WITHOUT_PIN_ERROR) {
                        //handle user pin 
                        Status = SessionStatus.UserPinRequired;
                    }
                    if( Remember == RememberOptions.HostLogin ) {
                        password = string.Empty;
                    } else if( Remember == RememberOptions.Host ) {
                        email = string.Empty;
                        password = string.Empty;
                    }
                    return false;
                }
            } else {
                object auth = null;
                if(Status == SessionStatus.AuthorizationCodeRequired) {
                    auth = AuthorizationCodeAuth();
                } else if(Status == SessionStatus.UserPinRequired) {
                    auth = UserPinAuth();
                } else {
                    throw new QuatrixAuthorizationException(new QuatrixExceptionArgs {
                        msg = Messages.Error.unsupported_authorizaion_type,
                        code = QuatrixException.NOT_SUPPORTED
                    });
                }
                //TODO: Handle other types of auth
                if(Post(SESSION_LOGIN, auth).Code != HttpStatusCode.OK) {
                    return false;           //invalid auth code
                }
            }
            Status = SessionStatus.PartialLoggedIn;
            try {
                Profile = this.GetProfile();               //Try to get profile data (failed if contact trying to login)
            }
            catch(WebException ex) {
                Debug.Logger.Error(SETTINGS.APP_LOG_NAME, ex, "WebException");
                Logout();
                throw;
            }
            if(Profile.is_contact) {
                Status = SessionStatus.ContactAuthorized;
                Logout();
                throw new QuatrixPermissionDeniedException();
            }
            Status = SessionStatus.LoggedIn;
            //remove user data depends from remember option
            if(Remember == RememberOptions.HostLogin) {
                password = string.Empty;
            } else if(Remember == RememberOptions.Host) {
                email = string.Empty;
                password = string.Empty;
            }
            //raise login event
            if(LoginedEvent != null) {
                LoginedEvent(this, new EventArgs());
            }
            return true;    //success
        }

        private object AuthorizationCodeAuth() {
            Validation.AuthorizaionCode(AuthCode);
            return new {
                auth_type = MFA_TYPE,
                value = new {
                    code = AuthCode
                }
            };
        }

        private object UserPinAuth() {
            Validation.UserPin(AuthCode);
            return new {
                auth_type = USER_PIN_TYPE,
                value = new {
                    pin = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(AuthCode))
                }
            };
        }

        /// <summary>
        /// Used to log off from account
        /// </summary>
        /// <returns>True if user is succesfully logged out. Otherwise - false</returns>
        public bool Logout() {
            KeepAlive = false;
            if(Status == SessionStatus.Unauthorized) {
                return true;
            }
            Status = SessionStatus.Unauthorized;
            try {
                Get(SESSION_LOGOUT);
            }
            catch(QuatrixNotAllowedException) {
                return true;
            }
            finally {
                RemoveToken();
                if(LogoutedEvent != null) {
                    LogoutedEvent(this, new EventArgs());
                }
            }
            return true;
        }

        protected virtual HttpResult OnUserLogin() {
            //TODO: handle log out if user was authorized
            //-------------------------------------------
            //Validate user data
            Validation.Email(email);
            Validation.Password(password);

            //Basic {base64(login:pass)} - format for authorization header
            string auth = string.Format("{0}:{1}", this.Email, this.Password);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(auth);
            auth = string.Concat("Basic ", Convert.ToBase64String(data));
            Headers.Clear();
            Headers.Add(HttpRequestHeader.Authorization, auth);
            HttpResult res = Get(SESSION_LOGIN);
            Session = ToEntity<Session>(res);
            Session.Request = this;
            Headers.Clear();
            Headers.Add(XAuthToken, Token);
            //TODO: handle response headers
            return res;
        }

        protected override void OnExceptionResponse(WebException ex, HttpWebResponse response) {
            if(response.StatusCode == HttpStatusCode.Unauthorized && Status == SessionStatus.LoggedIn) {
                if(KeepAlive && Remember == RememberOptions.All) {
                    Debug.Logger.Debug(SETTINGS.APP_LOG_NAME, "logging with saved user credetials");
                    //Handle expired session
                    if(Login(true)) {
                        Debug.Logger.Debug(SETTINGS.APP_LOG_NAME, "logged in with new session");
                        return;
                    }
                }
                throw new QuatrixAuthorizationException();
            }
            base.OnExceptionResponse(ex, response);   //return error depends on code
        }

        /// <summary>
        /// Used to request user password reset email.
        /// </summary>
        ///  <remarks> All parameters are validating  </remarks>
        /// <param name="host">Quatrix name where account is hosted</param>
        /// <param name="email">Address to send the email</param>
        /// <returns>True when request sending was successful</returns>
        public static string ResetPassword(string host, string email) {
            QSession session = new QSession(host, email);
            object obj = new {
                email = new[] { email }
            };
            return session.Post(RESET_PASSWORD, obj).Result;
        }

        protected void RemoveToken() {
            Status = SessionStatus.Unauthorized;
            Headers.Clear();
            Session = new Session();
        }

        void IDeserializationCallback.OnDeserialization(object sender) {
            sessionKeepAlive = new SessionKeepAlive(this);
            RemoveToken();
        }

        /// <summary>
        /// Dispose time and loguot
        /// </summary>
        public void Dispose() {
            sessionKeepAlive.Dispose();
            try {
                Logout();
            }
            catch(Exception ex) {
                Debug.Logger.Error(SETTINGS.APP_LOG_NAME, ex);
            }
            finally {
                RemoveToken();
            }
        }
    }
}