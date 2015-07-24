using Maytech.Quatrix.Messages;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Maytech.Quatrix.Tools.Net {
    
    public static class Processing {


        #region VALIDATION

        public static bool IsValidEmail ( this string email ) {
            if (!string.IsNullOrEmpty( email )) {
                try {
                    new System.Net.Mail.MailAddress( email.Trim() );
                    return true;
                }
                catch {
                }
            }
            return false;
        }


        public static bool IsValidEmailExc ( this string email ) {
            if (!email.IsValidEmail()) {
                throw new QException( Error.api_email_invalid );
            }
            else {
                return true;
            }
        }


        public static bool IsValidHost ( this string host ) {
            Uri uri;
            return host.IsValidHost( out uri );
        }


        public static bool IsValidHost ( this string host, out Uri uri ) {
            uri = null;
            //Check if host empty
            if (string.IsNullOrEmpty( host )) {
                throw new QException( Error.api_host_invalid );
            }
            //else try to validate string to host
            string adress = string.Empty;
            host = host.Trim();
            int pointCount = host.Count( c => c == '.' );

            if (pointCount > 1) {
                string address = host;
                string https = "https://";
                if (!host.StartsWith( https )) {
                    address = string.Format( "{0}{1}", https, address );
                }
                bool result = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create( address );
                request.Method = "HEAD";
                HttpWebResponse response = null;
                try {
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK) {
                        var rUri = response.ResponseUri;
                        UriBuilder bUri = new UriBuilder();
                        bUri.Host = rUri.Host;
                        bUri.Scheme = rUri.Scheme;
                        uri = new Uri( bUri.ToString() );
                        result = true;
                    }
                }
                catch {
                    throw new QException( Error.api_host_invalid );
                }
                finally {
                    if (response != null) {
                        response.Close();
                    }
                }
                return result;
            }
            else {
                throw new QException( Error.api_host_invalid );
            }
        }


        public static bool IsValidPassword ( this string password ) {
            password = password.Trim();
            return !( string.IsNullOrEmpty( password ) || password.Length < 6 || password.Length > 64 );
        }


        public static bool IsValidPasswordEx ( this string password ) {
            password = password.Trim();
            if (string.IsNullOrEmpty( password )) {
                throw new QException( Error.api_parameter_password_empty );
            }
            else if (password.Length < 6) {
                throw new QException( Error.api_parameter_passwortd_too_short );
            }
            else if (password.Length > 64) {
                throw new QException( Error.api_parameter_password_too_long );
            }
            else {
                return true;
            }
        }


        #endregion


        public static bool Enable ( this IQuatrixRequest request ) {
            return request.Enable( true );
        }


        public static bool Enable ( this IQuatrixRequest request, bool checkLogin ) {
            if (request == null) {
                var nex = new NullReferenceException();
                throw new QException( nex.Message );
            }
            else if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                throw new QException( Error.internet_not_connected );
            }
            else if (checkLogin && !request.IsLogined) {
                throw new QException( Error.api_not_logined );
            }
            else {
                return true;
            }
        }


        public static string CreateAnonymRequest ( string api_call, string error_message ) {
            try {
                string result = string.Empty;
                HttpWebRequest req = WebRequest.Create( api_call ) as HttpWebRequest;
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                return res.GetResult();
            }
            catch (WebException ex) {
                throw ex.CreateWebExcaption( error_message );
            }
        }


        public static string CreateAnonymRequest ( string api_call, byte[] data, string error_message ) {
            try {
                string result = string.Empty;
                HttpWebRequest req = WebRequest.Create( api_call ) as HttpWebRequest;
                req.Method = Method.POST.ToString();
                Stream stream = req.GetRequestStream();
                stream.Write( data, 0, data.Length );
                stream.Close();
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                return res.GetResult();
            }
            catch (WebException ex) {
                throw ex.CreateWebExcaption( error_message );
            }
        }


        public static string GetResult ( this HttpWebResponse response ) {
            string result = string.Empty;
            using (Stream stream = response.GetResponseStream()) {
                using (StreamReader reader = new StreamReader( stream )) {
                    result = System.Web.HttpUtility.HtmlDecode( reader.ReadToEnd() );
                }
            }
            return result;
        }


        internal static T ToEntity<T> ( this IQuatrixRequest request, string json ) where T : QEntity {
            if (string.IsNullOrEmpty( json )) {
                return default( T );
            }
            JToken token = JToken.Parse( json );
            T graph = token.ToObject<T>();
            graph.Request = request;
            return graph;
        }


        public static QException CreateWebExcaption ( this WebException ex ) {
            return ex.CreateWebExcaption( ex.Message );
        }


        public static QException CreateWebExcaption ( this WebException ex, string message ) {
            QException exception = new QException( message );
            if (ex.Response != null) {
                string serverMessage = string.Empty;
                using (var res = ex.Response.GetResponseStream()) {
                    if (!res.CanRead) {
                        return exception;
                    }
                    using (var read = new StreamReader( res )) {
                        serverMessage = read.ReadToEnd();
                    }
                }
                if (!string.IsNullOrEmpty( serverMessage )) {
                    try {
                        var token = JToken.Parse( serverMessage );
                        if (token != null) {
                            object maytech_error = new {
                                msg = string.Empty,
                                code = 0
                            };
                            Type t = maytech_error.GetType();
                            maytech_error = token.ToObject( t );
                            exception.msg = t.GetProperty( "msg" ).GetValue( maytech_error, null ).ToString();
                            exception.code = (int)t.GetProperty( "code" ).GetValue( maytech_error, null );
                        }
                    }
                    catch {
                        return new QException( Error.api_host_invalid );
                    }
                }
            }
            return exception;
        }


        public static T MakeApiCall<T> ( this IQuatrixRequest request, string api_call ) where T : QEntity {
            return request.MakeApiCall<T>( api_call, default( Action ) );
        }


        public static T MakeApiCall<T> ( this IQuatrixRequest request, string api_call, Action before ) where T : QEntity {
            request.Enable();
            if (before != null) {
                before();
            }
            return request.CreateRequest < T > ( api_call );
        }


        public static T MakeApiCall<T> ( this IQuatrixRequest request, string api_call, byte[] data ) where T : QEntity {
            return request.MakeApiCall<T>( api_call, data, null );
        }


        public static T MakeApiCall<T> ( this IQuatrixRequest request, string api_call, byte[] data, Action before ) where T : QEntity {
            request.Enable();
            if (before != null) {
                before();
            }
            return request.CreateRequest<T>( api_call, data );
        }
    }
}
