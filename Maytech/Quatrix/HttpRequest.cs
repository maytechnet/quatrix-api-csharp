using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Maytech.Quatrix {


    [Serializable]
    public abstract class HttpRequest {

        [field: NonSerialized]
        private static string userAgent =
            string.Format("QutrixAPI/{0}({1}) lib/{2}",
                QRequest.VersionApi,
                //os version
                Environment.OSVersion,
                //get asm version
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3));

        protected internal const int HOST_NOT_RESOLVED = -2146233079;

        protected const string CONTENT_TEXT = "text/plain";
        protected const string CONTENT_JSON = "application/json";
        protected const string CONTENT_HTML = "text/html";

        public abstract string Url { get; }

        /// <summary>
        /// User-Agent header for each Quatrix requests
        /// </summary>
        public string UserAgent {
            get {
                return userAgent;
            }
            set {
                userAgent = value;
            }
        }

        public abstract string Referer { get; }

        public int MaxAttempts { get; set; }

        protected WebHeaderCollection Headers { get; private set; }

        public HttpRequest() {
            MaxAttempts = SETTINGS.RECONNECT_COUNT;
            Headers = new WebHeaderCollection();
        }

        /// <summary>
        /// Send get http request
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        protected HttpResult Get(string route) {
            HttpWebRequest rq = Create(route);
            Init(rq);
            Debug.Logger.Debug(SETTINGS.APP_LOG_NAME, "send request; route:", route, ";method:", rq.Method);
            return Receive(rq);
        }

        /// <summary>
        /// Send post http request
        /// </summary>
        /// <param name="route"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected HttpResult Post(string route, byte[] data) {
            HttpWebRequest rq = Create(route);
            Init(rq, data);
            Debug.Logger.Debug(SETTINGS.APP_LOG_NAME, "send request; route:", route, ";method:", rq.Method);
            return Receive(rq, data);
        }

        /// <summary>
        /// Send post http request
        /// </summary>
        /// <param name="route"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        protected HttpResult Post(string route, object jsonData) {
            HttpWebRequest rq = Create(route);
            string json = JsonConvert.SerializeObject(jsonData, Formatting.None);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            Init(rq, buffer);
            Debug.Logger.Debug(SETTINGS.APP_LOG_NAME, "send request; route:", route, ";method:", rq.Method, ";data:", json);
            return Receive(rq);
        }

        /// <summary>
        /// Create request
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private HttpWebRequest Create(string route) {
            string uri = string.Concat(Url, route);
            return (HttpWebRequest)WebRequest.Create(uri);
        }

        /// <summary>
        /// Initialize request
        /// </summary>
        /// <param name="request"></param>
        private void Init(HttpWebRequest request) {
            request.Method = WebRequestMethods.Http.Get;
            request.Headers = Headers;
            request.Referer = Referer;
            request.UserAgent = UserAgent;
            OnSendRequest(request);
        }

        /// <summary>
        /// Initialize request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        private void Init(HttpWebRequest request, byte[] data) {
            Init(request);
            request.Method = WebRequestMethods.Http.Post;
            OnPostDataWrite(request, data);
        }

        private HttpResult Receive(HttpWebRequest request, byte[] data = null) {
            HttpWebResponse response = null;
            int attempt = 0;
            DateTime startTime = DateTime.MinValue;
            while(ReconnectCondition(attempt)) {
                attempt++;
                try {
                    startTime = DateTime.Now;
                    response = (HttpWebResponse)request.GetResponse();
                    Debug.Logger.Debug(SETTINGS.APP_LOG_NAME, "receive response; route:", request.RequestUri.PathAndQuery, ";method:", request.Method, ";elapsed(milliseconds):", (DateTime.Now - startTime).TotalMilliseconds);
                    return OnReceiveResponse(response);
                }
                catch(WebException ex) {
                    Debug.Logger.Info(SETTINGS.APP_LOG_NAME, "request failed: route:", request.RequestUri.PathAndQuery, ";message:", ex.Message, "; elapsed(milliseconds):", (DateTime.Now - startTime).TotalMilliseconds);
                    if(IsReconnectAllowed(ex)) {
                        System.Threading.Thread.Sleep(SETTINGS.RECONNECT_DELAY);  //sleep before reconnect
                    } else if(ex.Response is HttpWebResponse) {
                        //handle exception with http response
                        using(HttpWebResponse exresponse = (HttpWebResponse)ex.Response) {
                            OnExceptionResponse(ex, exresponse);
                        }
                    } else {
                        OnError(request, ex);
                    }
                }
                catch(Exception ex) {
                    Debug.Logger.Info(SETTINGS.APP_LOG_NAME, "request failed: route:", request.RequestUri.PathAndQuery, ";message:", ex.Message, "; elapsed(milliseconds):", (DateTime.Now - startTime).TotalMilliseconds);
                    OnError(request, ex);
                }
                finally {
                    if(response != null) {
                        response.Close();
                        response = null;
                    }
                }
                Debug.Logger.Debug(SETTINGS.APP_LOG_NAME, "reconnecting; attempt:", attempt, "; max allowed attempts:", MaxAttempts);
                //initialize new request
                request = (HttpWebRequest)WebRequest.Create(request.RequestUri);
                if(data == null) {
                    //int get request
                    Init(request);
                } else {
                    //init post request
                    Init(request, data);
                }
            }
            return HttpResult.Empty;
        }

        /// <summary>
        /// Raise before create request
        /// </summary>
        protected virtual void OnRequestValidate() { }

        /// <summary>
        /// Raise when web exception has <seealso cref="HttpWebResponse"/>
        /// </summary>
        /// <param name="ex">webexception</param>
        /// <param name="response">exception response</param>
        protected virtual void OnExceptionResponse(WebException ex, HttpWebResponse response) {
            throw ex;
        }

        /// <summary>
        /// Raise when request created and inited
        /// </summary>
        /// <param name="request"></param>
        protected virtual void OnSendRequest(HttpWebRequest request) { }

        /// <summary>
        /// Raise when response recieved
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual HttpResult OnReceiveResponse(HttpWebResponse response) {
            return ReadTextResponse(response);
        }

        /// <summary>
        /// Read text from response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected internal HttpResult ReadTextResponse(HttpWebResponse response) {
            using(Stream stream = response.GetResponseStream()) {
                using(StreamReader reader = new StreamReader(stream)) {
                    return new HttpResult(reader.ReadToEnd(), response.StatusCode, response.Headers);
                }
            }
        }

        /// <summary>
        /// Handle exception which raised in <seealso cref="HttpWebRequest.GetResponse"/>. 
        /// if this method doen't throw any exception request will be reconnected
        /// </summary>s
        /// <param name="request">current http request</param>
        /// <param name="ex">exception to throw</param>
        protected virtual void OnError(HttpWebRequest request, Exception ex) {
            throw ex;
        }

        /// <summary>
        /// Raise when 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="buffer"></param>
        protected virtual void OnPostDataWrite(HttpWebRequest request, byte[] buffer) {
            using(Stream stream = request.GetRequestStream()) {
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// Raise before <seealso cref="HttpWebRequest.GetResponse"/>, raise for each reconnect
        /// </summary>
        /// <param name="attempt"></param>
        /// <returns></returns>
        protected virtual bool ReconnectCondition(int attempt) {
            return attempt < MaxAttempts;
        }
    
        /// <summary>
        /// Check if on current exception allow to reconnect
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool IsReconnectAllowed(WebException ex) {
            return ex.Status == WebExceptionStatus.Timeout ||
                ex.Status == WebExceptionStatus.KeepAliveFailure;
        }
    }
}