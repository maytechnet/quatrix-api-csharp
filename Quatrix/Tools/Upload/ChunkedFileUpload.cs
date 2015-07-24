using Maytech.Quatrix.Tools.Net;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace Maytech.Quatrix.Tools.Upload {

    public delegate void ChunkedUploadProgress(object sender, UploadEventArgs e);
    public delegate void ChunkedUploadStatusChanged( object sender, UploadStatus e );

    
    public class ChunkedFileUpload : QFileRead, IQuatrixUpload, IDisposable {

        private readonly ManualResetEvent pauseEvent = new ManualResetEvent( true ); //Suspend and Resume event for upload
        private long sentTicks;
        private UploadStatus _status;
        private UploadStatus _last_status;
        private int _uBlockSize;
        private string uuid;
        private string uploadUri;

        public event ChunkedUploadProgress UploadProgressChanged;
        public event ChunkedUploadStatusChanged UploadStatusChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler UploadStarted;
        public event EventHandler UploadFinished;
        

        public UploadStatus CurrentStatus {
            get {
                return _status;
            }
            private set {
                if (_status != UploadStatus.Uploaded && _status != value) {
                    _last_status = _status;
                    _status = value;
                    if (UploadStatusChanged != null) {
                        UploadStatusChanged( this, _status );
                    }
                }
            }
        }


        public string SpecifiedPath {
            get {
                return this.CurrentFile.FullName;
            }
        }

        
        public string CurrentStatusFormated {
            get {
                return CurrentStatus.UploadStatusFormated();
            }
        }


        public override long BytesUploaded {
            get {
                return base.BytesUploaded;
            }
            protected set {
                base.BytesUploaded = value;
                if (UploadProgressChanged != null) {
                    UploadEventArgs args = new UploadEventArgs( BytesUploaded, Lenght, _uBlockSize, sentTicks );
                    UploadProgressChanged( this, args );
                } 
            }
        }


        public Metadata Parent {
            get;
            set;
        }


        public Metadata Result {
            get;
            private set;
        }


        public string SpecifiedName {
            get;
            set;
        }


        public long Length {
            get {
                return this.CurrentFile.Length;
            }
        }


        public ChunkedFileUpload( string path )
            : this(path, 4000000) {
        }


        public ChunkedFileUpload ( string path, int blockSize )
            : base( path, blockSize ) {
                _uBlockSize = 0;
                sentTicks = blockSize;
                System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
                this.CurrentStatus = UploadStatus.Waiting;
                this.SpecifiedName = this.CurrentFile.Name;
                this.UploadStarted += ChunkedFileUpload_UploadStarted; //ChunkedUpload_UploadStarted;
                this.UploadFinished += ChunkedUpload_UploadFinished;
        }

        private void ChunkedFileUpload_UploadStarted ( object sender, EventArgs e ) {
            CurrentStatus = UploadStatus.Upload;
            //Create upload link and init main prorty like upload uri and uuid
            string sfn = this.SpecifiedName;
            string tempPart = Operations.UploadAction.GetUploadUri( this.Parent, this.CurrentFile.FullName, out uuid, ref sfn );
            this.uploadUri = this.Parent.Request.CreateHostUri( tempPart );
            this.SpecifiedName = sfn;
        }


        private void ChunkedUpload_UploadFinished ( object sender, EventArgs e ) {
            if (this.CurrentStatus != UploadStatus.Canceled) {
                //Finilizing our upload
                this.Result = Operations.UploadAction.Finilize( Parent.Request, uuid, this.CurrentFile.Name );
                CurrentStatus = UploadStatus.Uploaded;
            }
        }


        private void NetworkChange_NetworkAvailabilityChanged( object sender, System.Net.NetworkInformation.NetworkAvailabilityEventArgs e ) {
            if (e.IsAvailable) {
                Resume();
            }
            else {
                Disconnect();
            }
        }


        public void Start ( ) {
            if (this.Parent == null) {
                throw new QException( "Parent metadata not set!" );
            }
            //Check on canceled data
            if (CurrentStatus == UploadStatus.Canceled) {
                return;
            }
            UploadStarted( this, new EventArgs() );                                     //occuring start event
            Stopwatch stopWatch = new Stopwatch();
            byte[] failedBlock = null;                                                  //block with failed upload data
            bool inFail = false;                                                        //value show if block upload was failed
            long offset = 0;
            int reconnect_count = 0;
            while (MoveNext && CurrentStatus != UploadStatus.Canceled) {
                stopWatch.Start();
                byte[] block = inFail ? failedBlock : GetBlock( out _uBlockSize );
                //Check if block upload failed
                offset = BytesUploaded + _uBlockSize;
                if (!Upload( block, this.uploadUri, offset )) {
                    //Block upload failed
                    inFail = true;
                    failedBlock = block;
                    _uBlockSize = 0;
                    if (++reconnect_count > 3) {
                        Disconnect();
                        reconnect_count = 0;
                    }
                    else {
                        System.Threading.Thread.Sleep( 500 );
                    }
                }
                else {
                    inFail = false;
                    reconnect_count = 0;
                }
                pauseEvent.WaitOne();
                BytesUploaded = offset;
                stopWatch.Stop();
                sentTicks = stopWatch.Elapsed.Ticks;
                stopWatch.Reset();
            }
            //Occuring finish event, if data was not canceled
            UploadFinished( this, new EventArgs() );
        }


        public void Suspend() {
            pauseEvent.Reset();
            CurrentStatus = UploadStatus.Suspended;
        }


        public void Resume() {
            switch (CurrentStatus) {
                case UploadStatus.Disconnected: {
                    pauseEvent.Set();
                    CurrentStatus = _last_status;
                    break;
                }
                case UploadStatus.Suspended: {
                    pauseEvent.Set();
                    CurrentStatus = UploadStatus.Upload;
                    break;
                }
                case UploadStatus.Canceled: {
                    if (_last_status == UploadStatus.Waiting) {
                        CurrentStatus = UploadStatus.Waiting;
                    }
                    break;
                }
            }
        }


        public void Stop() {
            this.CurrentStatus = UploadStatus.Canceled;
            pauseEvent.Set();
        }


        private bool Upload( byte[] data, string uri, long offset ) {
            HttpWebRequest request = WebRequest.Create( uri ) as HttpWebRequest;
            request.ContentLength = data.Length;
            request.Headers["Content-Range"] = string.Format( "bytes {0}-{1}/{2}", offset - data.Length, ( offset - 1 ), Lenght );
            request.Method = Method.POST.ToString();
            try {
                Stream stream = request.GetRequestStream();
                stream.Write( data, 0, data.Length );
                stream.Close();
                bool result = false;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
                    result = response.StatusCode == HttpStatusCode.OK;
                }
                return result;
            }
            catch (WebException ex) {
                var ex1 = ex.CreateWebExcaption();
                //if (ex1 != null) {

                //}
                return false;
                //exception stuff
            }
        }


        private void Disconnect ( ) {
            //pauseEvent.Reset();                       
            var tepmStatus = this.CurrentStatus;
            Suspend();                                  //suspend upload
            CurrentStatus = UploadStatus.Disconnected;  //Change status to disconnected
            this._last_status = tepmStatus;
        }
    }
}
