using System.Net;

namespace Maytech.Quatrix {


    public struct HttpResult {


        /// <summary>
        /// Result from response
        /// </summary>
        public readonly string Result;


        /// <summary>
        /// Http status core
        /// </summary>
        public readonly HttpStatusCode Code;


        /// <summary>
        /// Response headers collection
        /// </summary>
        public readonly WebHeaderCollection Headers;


        public static readonly HttpResult Empty = new HttpResult( string.Empty, QRequest.STATUS_UNKNOWN, new WebHeaderCollection() );


        public HttpResult( string json, HttpStatusCode code, WebHeaderCollection headers ) {
            Result = json;
            Code = code;
            Headers = headers;
        }
    }
}