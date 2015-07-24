using Maytech.Quatrix.Messages;
using Maytech.Quatrix.Tools.Net;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;

namespace Maytech.Quatrix {


    [Serializable]
    public abstract class QRequest : IQuatrixRequest {

        private bool _ServerCertificateValidationCallback = false;
        private WebHeaderCollection _headers = new WebHeaderCollection();

        //Main Quatrix Headers name
        internal const string XAuthLogin = "X-Auth-Login";
        internal const string XAuthTimestamp = "X-Auth-Timestamp";
        internal const string XAuthToken = "X-Auth-Token";

        public event RequestEventHandler SessionExpired;


        protected abstract SessionKeepAlive SessionExpire {
            get;
        }


        protected abstract string Referer {
            get;
            set;
        }


        protected string this[string headerName] {
            get {
                return _headers[headerName];
            }
            set {
                _headers[headerName] = value;
            }
        }


        public bool ServerCertificateValidationCallback {
            get {
                return _ServerCertificateValidationCallback;
            }
            set {
                ServicePointManager.ServerCertificateValidationCallback =
                    ( _ServerCertificateValidationCallback = value ) ?
                    new RemoteCertificateValidationCallback( delegate {
                    return true;
                } ) : null;
            }
        }


        public string ApiUri {
            get;
            private set;
        }


        protected abstract string CreateAuthorizeHeader ( string signature_subject );


        protected abstract string GetDefaultSignature ( string api_uri, Method method );


        protected virtual string CreateRequest ( string api_call, Method method, string signature_subject, byte[] postdata ) {
            //field whitch return result from request
            string json = string.Empty;
            if (string.IsNullOrEmpty( api_call )) {
                return json;
            }

            api_call = string.Format( "/{0}", api_call.TrimStart( ' ', '/' ) );
            //Set headers for request
            if (string.IsNullOrEmpty( signature_subject )) {
                signature_subject = GetDefaultSignature( api_call, method );
            }

            _headers[HttpRequestHeader.Authorization] = CreateAuthorizeHeader( signature_subject );
            //Create request 
            string uri = string.Format( "{0}{1}", ApiUri, api_call );
            HttpWebRequest request = WebRequest.Create( uri ) as HttpWebRequest;
            request.Method = method.ToString();
            request.Headers = _headers;
            request.Referer = Referer;
            try {
                if (method == Method.POST) {
                    Stream stream = request.GetRequestStream();
                    stream.Write( postdata, 0, postdata.Length );
                    stream.Close();
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                json = response.GetResult();
                if (!string.IsNullOrEmpty( response.Headers[XAuthToken] )) {
                    _headers[XAuthToken] = response.Headers[XAuthToken];
                }
                SessionExpire.Skip();
                response.Close();
                return json;
            }
            catch (WebException ex) {
                switch (ex.Status) {
                    case (WebExceptionStatus)401: {
                        if (SessionExpired != null) {
                            RequestEventArgs args = new RequestEventArgs( this, api_call, method, signature_subject, postdata );
                            SessionExpired( this, args );
                            return string.Empty;
                        }
                        throw ex.CreateWebExcaption();
                    }
                    case (WebExceptionStatus)404: {
                        throw ex.CreateWebExcaption( Error.api_not_logined );
                    }
                    case (WebExceptionStatus)500:
                    case (WebExceptionStatus)501:
                    case (WebExceptionStatus)502: {
                        throw ex.CreateWebExcaption( Error.api_connection_error_occured );
                    }
                    default: {
                        throw ex.CreateWebExcaption();
                    }
                }
            }
        }


        protected string CreateSignatureSubject ( Method method, string uri, params Header[] headers ) {
            StringBuilder strBuild = new StringBuilder();
            foreach (Header h in headers) {
                strBuild.Append( h.Name );
                strBuild.Append( ": " );
                strBuild.Append( h.Value );
                strBuild.Append( "\n" );
                _headers[h.Name] = h.Value;
            }
            return string.Format( "{0} {1}\n{2}", method.ToString(), uri, strBuild.ToString() );
        }


        protected void RemoveToken ( ) {
            _headers.Remove( XAuthToken );
        }


        protected void SetRequestData ( Uri host ) {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = host.Scheme;
            uriBuilder.Host = host.Host;
            Referer = uriBuilder.ToString();
            uriBuilder.Path = ( (IQuatrixRequest)this ).VersionApi;
            ApiUri = uriBuilder.ToString();
        }


        #region IQuatrixRequest


        string IQuatrixRequest.CreateRequest ( string api_call ) {
            return CreateRequest( api_call, Method.GET, null, null );
        }


        string IQuatrixRequest.CreateRequest ( string api_call, byte[] data ) {
            return CreateRequest( api_call, Method.POST, null, data );
        }


        string IQuatrixRequest.CreateRequest ( string api_call, Method method, string signature_subject, byte[] data ) {
            return CreateRequest( api_call, method, signature_subject, data );
        }


        T IQuatrixRequest.CreateRequest<T> ( string api_call ) {
            string json = CreateRequest( api_call, Method.GET, null, null );
            return this.ToEntity<T>( json );
        }


        T IQuatrixRequest.CreateRequest<T> ( string api_call, byte[] data ) {
            string json = CreateRequest( api_call, Method.POST, null, data );
            return this.ToEntity<T>( json );
        }


        T IQuatrixRequest.CreateRequest<T> ( string api_call, Method method, string signature_subject, byte[] data ) {
            string json = CreateRequest( api_call, method, signature_subject, data );
            return this.ToEntity<T>( json );
        }


        string IQuatrixRequest.CreateApiUri ( string api_call ) {
            Uri u = new Uri( this.Referer );
            UriBuilder ub = new UriBuilder();
            ub.Scheme = u.Scheme;
            ub.Host = u.Host;
            api_call = api_call.TrimStart( ' ', '/' );
            ub.Path = string.Format( "{0}/{1}", ( (IQuatrixRequest)this ).VersionApi, api_call );
            return ub.ToString();
        }


        public abstract bool IsLogined {
            get;
            protected set;
        }


        string IQuatrixRequest.Host {
            get {
                return Referer;
            }
        }


        string IQuatrixRequest.CreateHostUri ( string api_call ) {
            Uri u = new Uri( this.Referer );
            UriBuilder ub = new UriBuilder();
            ub.Scheme = u.Scheme;
            ub.Host = u.Host;
            ub.Path = api_call;
            return ub.ToString();
        }


        string IQuatrixRequest.VersionApi {
            get {
                return "/api/0.1";
            }
        }

        #endregion
    }
}