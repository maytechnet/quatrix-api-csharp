using System;
using System.Net;

namespace Maytech.Quatrix {

    /// <summary>
    /// Provides the possibility to create requests and work with them
    /// </summary>
    [Serializable]
    public abstract class QRequest : HttpRequest, IQuatrixRequest {


        public const string DEFAULT_HTTP_SCHEMA = "https://";
        public const string DEFAULT_QUATRIX_DOMAIN_NAME = "quatrix.it";


        #region API CALLS

        public const string ACCOUNT_METADATA = "/account/metadata";

        public const string RESET_PASSWORD = "/reset-password/request";

        public const string SESSION_LOGIN = "/session/login";
        public const string SESSION_LOGOUT = "/session/logout";
        public const string SESSION_2FA = "/session/login-2fa";
        public const string SESSION_KEEP_ALIVE = "/session/keepalive";

        public const string PROFILE_INFO = "/profile/info";

        /// <summary>
        /// Format this value with 2 params. 1 - metadata.id, 2 - return content
        /// </summary>
        public const string FILE_METADATA = "/file/metadata/{0}?content={1}";
        public const string FILE_MKDIR = "/file/makedir";
        public const string FILE_DOWNLOAD_LINK = "/file/download-link";
        /// <summary>
        /// Concat this route with metadata file ID
        /// </summary>
        public const string FILE_DOWNLOAD = "/file/download/";
        public const string FILE_DELETE = "/file/delete";
        /// <summary>
        /// Concat this route with metadata file ID
        /// </summary>
        public const string FILE_RENAME = "/file/rename/";
        public const string FILE_MOVE = "/file/move";

        public const string CONTACT_CREATE = "/contact/create";
        public const string CONTACT_LIST = "/contact";

        public const string SHARE_CREATE = "/share/create";
        /// <summary>
        /// Concat this route with share ID
        /// </summary>
        public const string SHARE_DOWNLOAD_INFO = "/share/download-info/";
        /// <summary>
        /// Concat this route with share ID
        /// </summary>
        public const string SHARE_FILES = "/share/files/";
        /// <summary>
        /// Concat this route with referer in begin and share ID in end
        /// </summary>
        public const string SHARE_DOWNLOAD = "/download/";
        /// <summary>
        /// Concat this route with referer in begin and share ID in end
        /// </summary>
        public const string SHARE_RETURN_FILES = "/files-return/";

        public const string UPLOAD_LINK = "/upload/link";
        /// <summary>
        /// Concat this route with file metadata id
        /// </summary>
        public const string UPLOAD_MODIFY = "/file/modify/";
        /// <summary>
        /// Concat this route with upload key
        /// </summary>
        public const string UPLOAD_FINILIZE = "/upload/finalize/";
        /// <summary>
        /// Concat this route with upload key
        /// </summary>
        public const string UPLOAD_CHUNKED = "/upload/chunked/";

        /// <summary>
        /// Concat this route with session id
        /// </summary>
        public const string MQ_REQUEST = "/mq/req?id=";

        #endregion

        #region CUSTOM HTTP STATUS CODES

        public const HttpStatusCode STATUS_UNKNOWN = (HttpStatusCode)(-1);
        public const HttpStatusCode STATUS_MULTI_STATUS = (HttpStatusCode)207;

        #endregion

        /// <summary>
        /// Gets current api version; 
        /// </summary>
        /// <value> 0.1</value>
        public const string VersionApi = "1.0";

        ////Main Quatrix Headers name
        //internal const string XAuthLogin = "X-Auth-Login";
        //internal const string XAuthTimestamp = "X-Auth-Timestamp";
        internal const string XAuthToken = "X-Auth-Token";


        private readonly string api_url;            //api url
        private readonly string host_url;           //account url


        public abstract string Token { get; }


        public sealed override string Url {
            get {
                return api_url;
            }
        }


        public sealed override string Referer {
            get {
                return host_url;
            }
        }


        string IQuatrixRequest.ApiUri {
            get {
                return Url;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="QRequest" /> class, with the specified host name.
        /// </summary>
        /// <param name="host">Quatrix name where account is hosted</param>
        public QRequest( string host ) {
            Uri apiUri = null;
            if( !Uri.TryCreate( host, UriKind.Absolute, out apiUri ) ) {
                if( !host.Contains( "." ) ) {
                    host = string.Concat( host, ".", DEFAULT_QUATRIX_DOMAIN_NAME );
                }
                UriBuilder builder = new UriBuilder( DEFAULT_HTTP_SCHEMA, host, -1 );
                apiUri = builder.Uri;
            }
            if( new Uri( host ).Scheme == Uri.UriSchemeHttp ) {
                UriBuilder builder = new UriBuilder( host ) {
                    Scheme = Uri.UriSchemeHttps,
                    Port = -1
                };
                apiUri = builder.Uri;
            }
            UriBuilder b = new UriBuilder( apiUri );
            host_url = apiUri.ToString().TrimEnd( ' ', '/' );
            b.Path = string.Concat( b.Path, "api/", VersionApi );
            api_url = b.Uri.ToString().TrimEnd( ' ', '/' );
        }


        protected override void OnRequestValidate() {
            base.OnRequestValidate();
        }


        protected override void OnExceptionResponse( WebException ex, HttpWebResponse response ) {
            //try to create Quatrix Exception
            if( response.ContentType.Contains( CONTENT_JSON ) ) {
                HttpResult res = ReadTextResponse( response );
                QuatrixExceptionArgs args = QuatrixExceptionArgs.Create( res );
                throw QuatrixException.Create( args, ex );
            }
            //occurs base
            base.OnExceptionResponse( ex, response );
        }


        internal T ToEntity<T>( HttpResult res ) {
            if( string.IsNullOrEmpty( res.Result ) ) {
                return default( T );
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>( res.Result );
        }


        #region IQuatrixRequest


        string IQuatrixRequest.Get( string route ) {
            return Get( route ).Result;
        }


        T IQuatrixRequest.Get<T>( string route ) {
            HttpResult res = Get( route );
            T graph = ToEntity<T>( res );
            graph.Request = this;
            return graph;
        }


        string IQuatrixRequest.Post( string route, object data ) {
            return Post( route, data ).Result;
        }


        T IQuatrixRequest.Post<T>( string route, object data ) {
            HttpResult res = Post( route, data );
            T graph = ToEntity<T>( res );
            graph.Request = this;
            return graph;
        }


        #endregion
    }
}