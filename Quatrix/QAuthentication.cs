using Maytech.Quatrix.Tools.Net;
using Maytech.Quatrix.Tools.Security;
using Maytech.Quatrix.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Maytech.Quatrix.Messages;
using Maytech.Quatrix.Tools;

namespace Maytech.Quatrix {

    [Serializable]
    public class QAuthentication : QRequest, IQEntity, IQuatrixAuthentication, IDeserializationCallback {

        //User data
        private string encPass;
        private SessionKeepAlive expireEvent;
        private Profile profile;



        /// <summary>
        /// Event occuring when login successfully
        /// </summary>
        public event EventHandler LoginedEvent;


        /// <summary>
        /// Event occuring when login failed
        /// </summary>
        public event EventHandler LoginFailedEvent;


        /// <summary>
        /// Event occuring when call logout
        /// </summary>
        public event EventHandler LogoutedEvent;


        public RememberOptions Remember {
            get;
            set;
        }


        /// <summary>
        /// Return current user 
        /// </summary>
        public Profile Profile {
            get {
                if (profile == null) {
                    profile = this.GetProfile();
                }
                return profile;
            }
        }


        public string Host {
            get;
            set;
        }


        public string Email {
            get;
            set;
        }


        public string Password {
            get;
            set;
        }


        public bool KeepAlive {
            get {
                return this.SessionExpire.KeepAlive;
            }
            set {
                this.SessionExpire.KeepAlive = value;
            }
        }


        public override bool IsLogined {
            get;
            protected set;
        }


        protected override string Referer {
            get;
            set;
        }


        protected override SessionKeepAlive SessionExpire {
            get {
                return this.expireEvent;
            }
        }


        public QAuthentication ( ) {
            Remember = RememberOptions.HostLogin;
            ServerCertificateValidationCallback = true;
            this.expireEvent = new SessionKeepAlive( this );
            this.SessionExpire.KeepAlive = true;
            this.LoginedEvent += QuatrixAuthentication_LoginedEvent;
            this.SessionExpired += QuatrixAuthentication_SessionExpired;
        }


        public QAuthentication ( string host, string email )
            : this() {
            Uri u;
            if (host.IsValidHost( out u ) && email.IsValidEmailExc()) {
                this.Host = u.ToString();
                this.Email = email;
                this.SetRequestData( u );
            }
        }


        public QAuthentication ( string host, string email, string password )
            : this( host, email ) {
            if (password.IsValidPasswordEx()) {
                Password = password;
            }
        }


        #region Declareted session events


        private void QuatrixAuthentication_LoginedEvent ( object sender, EventArgs e ) {
            if (this.Profile.is_contact) {
                throw new QException( Error.api_login_not_allowed );
            }
            this.SessionExpire.Start();
        }


        private void QuatrixAuthentication_SessionExpired ( object sender, RequestEventArgs args ) {
            if (this.IsLogined) {
                //Here we try to resolve last failed request
                //Only in if user keeping session and he was authorized
                if (this.KeepAlive) {
                    this.Login();
                    this.CreateRequest( args.ApiCall, args.Method, args.Signature, args.PostData );
                }
                else {
                    throw new QException( Error.api_session_expired );
                }
            }
            else {
                throw new QException( Error.api_not_logined );
            }
        }


        #endregion


        #region Public method

        /// <summary>
        /// Valid field and login 
        /// </summary>
        /// <exception cref="QuatrixException"></exception>
        /// <returns></returns>
        public bool Login ( ) {
            this.Enable( false );
            if (IsLogined) {
                return true;
            }
            ValidateFields();
            //Prepare data
            SetRequestData( new Uri( Host ) );
            this.encPass = EncPass( Password );
            //URI for authorization
            string URI = "/session/login";

            Header hTimestamp = new Header {
                Name = QRequest.XAuthTimestamp.ToLower(),
                Value = Functions.GetTimestamp()
            };

            Header hLogin = new Header {
                Name = QRequest.XAuthLogin.ToLower(),
                Value = Email
            };

            string signature = CreateSignatureSubject( Method.GET, URI, hLogin, hTimestamp );
            Account ac = ( (IQuatrixRequest)this ).CreateRequest<Account>( URI, Method.GET, signature, null );
            IsLogined = !string.IsNullOrEmpty( ac.account_id );
            if (IsLogined) {
                LoginedEvent( this, new EventArgs() );
            }
            else {
                RemoveToken();
                if (LoginFailedEvent != null) {
                    LoginFailedEvent( this, new EventArgs() );
                }
                else {
                    throw new QException( Error.api_not_logined );
                }
            }
            return IsLogined;
        }


        public bool CheckPassword ( string password ) {
            if (!string.IsNullOrEmpty( this.Password )) {
                if (password.IsValidPasswordEx()) {
                    return encPass == this.EncPass( password );
                }
                return false;
            }
            else {
                throw new QException( Error.api_parameter_password_empty );
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>true on out</returns>
        public bool Logout ( ) {
            Reset();
            if (LogoutedEvent != null) {
                LogoutedEvent( this, new EventArgs() );
            }
            return !IsLogined;
        }


        public bool ValidateFields ( ) {
            return ValidateFields( Host, Email, Password );
        }


        /// <summary>
        /// Validate user data fields
        /// Can chage host name
        /// </summary>
        /// <param name="host"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateFields ( string host, string email, string password ) {
            try {
                Uri u;
                if (Processing.IsValidHost( host, out u )) {
                    Host = u.ToString();
                }
                Processing.IsValidEmailExc( email );
                Processing.IsValidPasswordEx( password );
                return true;
            }
            catch (QException ex) {
                throw ex;
            }
        }


        #endregion


        #region Closed methods

        protected override string CreateAuthorizeHeader ( string signature_subject ) {
            return SecurityTools.HMAC_SHA1( signature_subject, encPass );
        }


        protected override string GetDefaultSignature ( string api_uri, Method method ) {
            Header hToken = new Header() {
                Name = XAuthToken.ToLower(),
                Value = this[XAuthToken]
            };

            Header hTimestamp = new Header() {
                Name = XAuthTimestamp.ToLower(),
                Value = Functions.GetTimestamp()
            };

            this[XAuthTimestamp] = hTimestamp.Value;

            return CreateSignatureSubject( method, api_uri, hTimestamp, hToken );
        }


        private string EncPass ( string password ) {
            byte[] encPass = SecurityTools.GetPBKDF2( password );
            return SecurityTools.GetHexPassword( encPass );
        }


        private void Reset ( ) {
            IsLogined = false;
            RemoveToken();
            if (Remember == RememberOptions.All) {
                return;
            }
            Password = string.Empty;
            encPass = string.Empty;
            if (Remember == RememberOptions.HostLogin) {
                return;
            }
            Host = string.Empty;
            Email = string.Empty;
        }

        #endregion


        #region Implementing interfaces

        void IDeserializationCallback.OnDeserialization ( object sender ) {
            ServerCertificateValidationCallback = ServerCertificateValidationCallback;
            Reset();
        }


        bool IQuatrixAuthentication.KeepAlive ( ) {
            try {
                IQuatrixRequest request = this;
                request.Enable();
                string api_call = "session/keepalive";
                request.CreateRequest( api_call );
                return true;
            }
            catch {
                Logout();
                return false;
            }
        }



        #region IQEntity


        IQuatrixRequest IQEntity.Request {
            get {
                return this;
            }
        }


        string IQEntity.id {
            get {
                return Profile.id;
            }
        }

        #endregion


        #endregion
    }
}
