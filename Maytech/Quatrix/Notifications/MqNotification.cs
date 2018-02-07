using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Maytech.Quatrix.Notifications {


    /// <summary>
    /// Class which listen mq notification
    /// </summary>
    public class MqNotification : HttpRequest, IDisposable {


        public const string HEADER_ETAG_KEY = "Etag";
        public const string HEADER_LAST_MODIFIED_KEY = "Last-Modified";

        private readonly string notification_route;
        private readonly Thread worker;
        private HttpWebRequest currentRequest = null;


        public virtual QApi Api { get; private set; }


        public virtual bool IsListening { get; private set; }


        protected int Timeout { get; set; }


        public override string Url {
            get {
                return Api.Referer;
            }
        }


        public override string Referer {
            get {
                return string.Empty;
            }
        }


        protected DateTime IfModifiedSince { get; private set; }

        public MqNotification( QApi api, long modifiedSince ) {
            Api = api;
            worker = new Thread( () => {
                Func<string, HttpResult> gn = Get;
                gn.Invoke( notification_route );
            } );
            IfModifiedSince = DateTime.FromFileTimeUtc( modifiedSince );
            notification_route = string.Concat( QRequest.MQ_REQUEST, api.Session.id );
        }


        public virtual void Start() {
            if( IsListening ) {
                return;
            }
            IsListening = true;
            worker.Start();
        }


        protected override void OnSendRequest( HttpWebRequest request ) {
            currentRequest = request;
            base.OnSendRequest( request );
            request.IfModifiedSince = IfModifiedSince;
            request.Headers.Add( HttpRequestHeader.IfNoneMatch, Headers[HttpRequestHeader.IfNoneMatch] );
        }


        protected sealed override HttpResult OnReceiveResponse( HttpWebResponse response ) {
            IfModifiedSince = response.LastModified;
            Headers[HttpRequestHeader.IfNoneMatch] = response.Headers[HEADER_ETAG_KEY];
            return HandleResponse( response );
        }


        protected virtual HttpResult HandleResponse( HttpWebResponse response ) {
            return base.OnReceiveResponse( response );
        }


        protected override bool ReconnectCondition( int attempt ) {
            return IsListening;
        }


        protected override void OnError( HttpWebRequest request, Exception ex ) {
            base.OnError( request, ex );
        }


        public void Dispose() {
            IsListening = false;
            if( currentRequest != null && !currentRequest.HaveResponse ) {
                currentRequest.Abort();
                currentRequest = null;
            }
            if( worker != null ) {
                worker.Interrupt();
            }
        }
    }
}