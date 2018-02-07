using Maytech.Quatrix.Entity;
using System;
using System.IO;
using System.Net;

namespace Maytech.Quatrix.Transfer {

    public class DownloadMetadata : HttpRequest, IQuatrixTransfer {

        public const int BUFFER_SIZE = 16 * 1024;

        /// <summary>
        /// Occurs when download is started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when download is finished.
        /// </summary>
        public event EventHandler Finished;

        public IQMetadata Metadata { get; private set; }

        public override string Url {
            get {
                return Metadata.Request.ApiUri;
            }
        }

        public long Lenght { get; private set; }

        public string FullName { get; set; }

        public override string Referer {
            get {
                return string.Empty;
            }
        }

        public DownloadMetadata( IQMetadata metadata ) {
            Metadata = metadata;
            Headers.Add( QRequest.XAuthToken, metadata.Request.Token );
        }

        protected virtual string OnGetDownloadRoute() {
            object ids = new {
                ids = new string[] { Metadata.id }
            };
            IQuatrixRequest req = Metadata.Request;
            Id id = req.Post<Id>( QRequest.FILE_DOWNLOAD_LINK, ids );
            return string.Concat( QRequest.FILE_DOWNLOAD, id.id );
        }

        public virtual void Start() {
            if( Started != null ) {
                Started( this, new EventArgs() );
            }
            if( string.IsNullOrEmpty( FullName ) ) {
                throw new ArgumentNullException( "FullName" );
            }
            OnDownload();
            if( Finished != null ) {
                Finished( this, new EventArgs() );
            }
        }

        public virtual void OnDownload() {
            if(Metadata.Type == MetadataType.FILE) {
                string dwnroute = OnGetDownloadRoute();
                OnFileDownload(dwnroute);
            } else {
                OnFolderDownload();
            }
        }

        protected virtual void OnFileDownload( string route ) {
            Get( route );
        }

        protected virtual void OnFolderDownload() {
            Directory.CreateDirectory( FullName );
        }

        protected override HttpResult OnReceiveResponse( HttpWebResponse response ) {
            using( Stream stream = response.GetResponseStream() ) {
                using( Stream file = new FileStream( FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite ) ) {
                    byte[] buffer = new byte[BUFFER_SIZE]; // Fairly arbitrary size
                    int bytesRead;
                    long totalBytesReaded = 0;
                    while( (bytesRead = stream.Read( buffer, 0, buffer.Length )) > 0 ) {
                        file.Write( buffer, 0, bytesRead );
                        totalBytesReaded += bytesRead;
                    }
                    if( totalBytesReaded != response.ContentLength ) {
                        throw new IOException( "Read bytes aren't equal to response content length" );
                    }
                    Lenght = totalBytesReaded;
                    return new HttpResult( string.Empty, response.StatusCode, response.Headers );
                }
            }
        }
    }
}