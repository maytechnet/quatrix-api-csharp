using Maytech.Quatrix.Messages;
using System;
using System.IO;
using System.Net;

namespace Maytech.Quatrix.Transfer {

    public class FileChunkUpload : HttpRequest, IQuatrixTransfer, IDisposable {


        private Metadata parent;

        private FileStream fileStream;
        private long position;


        protected virtual int ChunkSize { get; private set; }


        /// <summary>
        /// Occurs when uploading is started.
        /// </summary>
        public event EventHandler Started;


        /// <summary>
        /// Occurs when uploading is finished.
        /// </summary>
        public event EventHandler Finished;


        /// <summary>
        /// Gets information about uploaded file or folder.
        /// </summary>
        public Metadata Result { get; private set; }


        protected bool ResolveName { get; set; }


        protected internal string ParentId { get; set; }


        protected internal IQuatrixRequest Session { get; set; }


        public override string Url {
            get {
                return Session.Referer;
            }
        }


        public override string Referer {
            get {
                return string.Empty;
            }
        }


        /// <summary>
        /// Gets information about current file.
        /// </summary>
        public FileInfo CurrentFile { get; private set; }


        /// <summary>
        /// Gets file size;
        /// </summary>
        public long Length {
            get {
                return CurrentFile.Length;
            }
        }


        /// <summary>
        /// Gets a value indicating whether [file is uploading].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [move next]; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool MoveNext {
            get {
                return position < CurrentFile.Length;
            }
        }


        /// <summary>
        /// Gets the full path of file.
        /// </summary>
        public string FullName {
            get {
                return CurrentFile.FullName;
            }
        }


        /// <summary>
        /// Gets position of uploaded bytes.
        /// </summary>
        /// </summary>
        public virtual long Position {
            get {
                return position;
            }
            private set {
                if( value < 1 ) {
                    throw new ArgumentException( "file postion less than zero" );
                }
                position = value;
                OnPositionChange( position );
            }
        }


        protected internal FileChunkUpload( string path, int chunkSize ) {
            ChunkSize = chunkSize;
            ResolveName = true;
            position = 0;
            CurrentFile = new FileInfo( path );
        }


        /// <summary>
        /// Gets or sets the path on cloud where files should be uploaded.
        /// </summary>
        public Metadata Parent {
            get {
                return parent;
            }
            set {
                parent = value;
                if( parent != null ) {
                    this.ParentId = parent.id;
                    this.Session = parent.Request;
                }
            }
        }


        /// <summary>
        /// Starts file uploading. Sets status to "Upload". After upload (success or with error) FileStream will close
        /// </summary>
        /// <exception cref="QuatrixWebException">If parent metadata is empty</exception>
        public virtual void Start() {
            if( string.IsNullOrEmpty( ParentId ) ) {
                throw new ArgumentNullException( "Parent", Error.api_parent_metadata_null );
            }
            if( Started != null ) {
                Started( this, new EventArgs() );
            }
            if( Parent == null ) {
                Parent = new Metadata { id = ParentId, Request = Session };
            }
            UploadInfo uploadInfo = GetUploadInfo();
            if( uploadInfo == null ) {
                throw new InvalidOperationException( "Failed to get upload info" );
            }
            string route = uploadInfo.GetUploadRoute();           //get upload uri from INFO
            try {
                fileStream = CurrentFile.OpenRead();
                while( MoveNext ) {
                    Post( route, GetChunk() );
                }
            }
            catch( Exception ex ) {
                OnUploadException( ex );
            }
            finally {
                Dispose();
            }
            OnUploadFinilize( uploadInfo );                              //finilize uploading
            OnFinish();
        }


        /// <summary>
        /// Upload info parameters
        /// </summary>
        /// <exception cref="QuatrixWebException"></exception>
        /// <returns></returns>
        protected virtual UploadInfo GetUploadInfo() {
            //object will ref chunked upload
            object u = new {
                parent_id = ParentId,
                file_size = Length,
                name = CurrentFile.Name,
                resolve = ResolveName
            };
            //Create upload link and init main prorty like upload uri and uuid
            UploadInfo uploadInfo = Session.Post<UploadInfo>( QRequest.UPLOAD_LINK, u );
            Result = new Metadata { id = uploadInfo.id, name = uploadInfo.name, parent_id = uploadInfo.parent_id, Parent = this.Parent };
            return uploadInfo;
        }


        /// <summary>
        /// Send finilize api request (update <paramref name="Result"/> value)
        /// </summary>
        /// <param name="uploadInfo">file upload info</param>
        protected virtual void OnUploadFinilize( UploadInfo uploadInfo ) {
            //Finalizing our upload
            string route = string.Concat( QRequest.UPLOAD_FINILIZE, uploadInfo.upload_key );
            //Create get request to finilize upload
            Result = Session.Get<Metadata>( route );
            uploadInfo.Parent = Result.Parent;
        }


        /// <summary>
        /// Change uploaded bytes count from read position
        /// </summary>
        /// <param name="position"></param>
        protected virtual void OnPositionChange( long position ) { }


        /// <summary>
        /// Gets the block of bytes to upload
        /// </summary>
        /// <param name="length">Size of one data block.</param>
        /// <returns>Bytes array, that represents one block of data to upload</returns>
        protected virtual byte[] GetChunk() {
            long leaving_size = Length - position;
            ChunkSize = leaving_size < ChunkSize ? (int)leaving_size : ChunkSize;
            byte[] buffer = new byte[ChunkSize];
            fileStream.Seek( position, SeekOrigin.Begin );
            fileStream.Read( buffer, 0, ChunkSize );
            return buffer;
        }


        protected override void OnSendRequest( HttpWebRequest request ) {
            base.OnSendRequest( request );
            request.ContentLength = ChunkSize;
            //upload_position - chunk.Length, (upload_position - 1), Length );
            request.Headers["Content-Range"] = string.Format( "bytes {0}-{1}/{2}", Position, (Position + ChunkSize - 1), Length );
        }


        protected override HttpResult OnReceiveResponse( HttpWebResponse response ) {
            if( response.StatusCode == HttpStatusCode.OK ) {
                Position += ChunkSize;
                return new HttpResult( string.Empty, response.StatusCode, response.Headers );
            }
            return HttpResult.Empty;
        }


        protected virtual void OnUploadException( Exception ex ) {
            throw ex;
        }


        protected override void OnExceptionResponse(WebException ex, HttpWebResponse response) {
            //try to create Quatrix Exception
            if(response.ContentType.Contains(CONTENT_JSON)) {
                HttpResult res = ReadTextResponse(response);
                QuatrixExceptionArgs args = QuatrixExceptionArgs.Create(res);
                throw QuatrixException.Create(args, ex);
            }
            base.OnExceptionResponse(ex, response);
        }


        protected override void OnError( HttpWebRequest request, Exception ex ) {
            if( ex is IOException ) {
                // try to handle this exception: "Unable to write data to the transport connection: An existing connection was forcibly closed by the remote host."
                Debug.Logger.Error( SETTINGS.APP_LOG_NAME, ex, "IOException" );
                return;
            }
            base.OnError( request, ex );
        }


        protected virtual void OnFinish() {
            //reise finish event
            if( this.Finished != null ) {
                this.Finished( this, new EventArgs() );
            }
        }


        public void Dispose() {
            if( fileStream != null ) {
                fileStream.Close();
                fileStream = null;
            }
        }
    }
}