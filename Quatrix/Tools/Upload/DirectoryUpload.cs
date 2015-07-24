using Maytech.Quatrix.Messages;
using Maytech.Quatrix.Operations;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace Maytech.Quatrix.Tools.Upload {

    //Implemented directory upload 
    //Using adapter pattern in special way...

    [Serializable]
    public sealed class DirectoryUpload : IQuatrixUpload, IDisposable {

        private ChunkedFileUpload curUpl;
        private UploadStatus uplStatus;
        private System.Threading.ManualResetEvent pause = new System.Threading.ManualResetEvent( false );
        //EVENTS

        public event ChunkedUploadProgress UploadProgressChanged;
        public event ChunkedUploadStatusChanged UploadStatusChanged;
        public event EventHandler UploadStarted;
        public event EventHandler UploadFinished;



        public UploadStatus CurrentStatus {
            get {
                return uplStatus;
            }
            private set {
                if (uplStatus != value) {
                    uplStatus = value;
                    if (this.UploadStatusChanged != null) {
                        this.UploadStatusChanged( this, value );
                    }
                }
            }
        }


        public string CurrentStatusFormated {
            get {
                return CurrentStatus.UploadStatusFormated();
            }
        }


        public long BytesUploaded {
            get;
            private set;
        }


        public Metadata Parent {
            get;
            set;
        }


        public Metadata Result {
            get;
            private set;
        }


        public bool Recursive {
            get;
            set;
        }


        public string SpecifiedName {
            get;
            set;
        }


        public string SpecifiedPath {
            get;
            private set;
        }


        public long Length {
            get;
            private set;
        }


        public DirectoryInfo CurrentDirectory {
            get;
            private set;
        }


        public DirectoryUpload ( string path ) {
            this.SpecifiedPath = path;
            this.CurrentDirectory = new DirectoryInfo( path );
            if (!this.CurrentDirectory.Exists) {
                throw new QException( Error.directory_not_exist );
            }
            this.SpecifiedName = this.CurrentDirectory.Name;
            this.Recursive = true;
            //Calculate dir lenght          
            try {
                this.Length = this.CalculateDirSize( this.CurrentDirectory );
            }
            catch (Exception ex) {
                throw new QException( ex.Message );
            }
            this.UploadStarted += ChunkedDirectoryUpload_UploadStarted;
            this.UploadFinished += ChunkedDirectoryUpload_UploadFinished;
        }

        private void ChunkedDirectoryUpload_UploadFinished ( object sender, EventArgs e ) {
            if (this.CurrentStatus != UploadStatus.Canceled) {
                this.CurrentStatus = UploadStatus.Uploaded;
            }
        }


        private void ChunkedDirectoryUpload_UploadStarted ( object sender, EventArgs e ) {
            //Check if all great! 
            if (this.Parent == null) {
                throw new QException( "Parent directory not set!" );
            }
            this.CurrentStatus = UploadStatus.Upload;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri">this uri work as local directory path</param>
        public void Start ( ) {
            if (this.CurrentStatus != UploadStatus.Canceled ) {
                this.UploadStarted( this, new EventArgs() );
                try {
                    if (this.Length > 0) {
                        Uploading( this.Parent, this.CurrentDirectory, this.SpecifiedName );
                    }
                }
                catch (QException ex) {
                    throw ex;
                }
                UploadFinished( this, new EventArgs() );
            }
        }


        private void Uploading ( Metadata parent, DirectoryInfo dirInfo, string dirName ) {
            if (CurrentStatus == UploadStatus.Canceled) {
                return;
            }
            //Begin work
            Metadata mtd = parent.MakeDir( dirName );
            if (this.Result == null) {
                this.Result = mtd;
            }
            FileInfo[] fs = dirInfo.GetFiles();
            //Upload files from currunt directory
            foreach (var f in fs) {
                using (curUpl = new ChunkedFileUpload( f.FullName )) {
                    curUpl.UploadStatusChanged += curUpl_UploadStatusChanged;
                    curUpl.UploadProgressChanged += curUpl_UploadProgressChanged;
                    mtd.Upload( curUpl );
                }
            }
            //Check if allowed recursive uploading
            if (this.Recursive) {
                DirectoryInfo[] ds = dirInfo.GetDirectories();
                foreach (var d in ds) {
                    //Skip all empty dirs
                    if (CalculateDirSize( d ) > 0) {
                        Uploading( mtd, d, d.Name );
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void curUpl_UploadProgressChanged ( object sender, UploadEventArgs e ) {
            this.BytesUploaded += e.BlockSize;
            if (this.UploadProgressChanged != null) {
                UploadEventArgs args = new UploadEventArgs( this.BytesUploaded, this.Length, e.BlockSize, e.sentTicks );
                this.UploadProgressChanged( this, args );
            }
        }



        internal long CalculateDirSize ( DirectoryInfo dir ) {
            long lenght = 0;                                //result variable
            lenght = dir.GetFiles().Sum( s => s.Length );   //file lenght
            //dir lenght
            foreach (var d in dir.GetDirectories()) {
                lenght += CalculateDirSize( d );
            }
            return lenght;                                  //our result lenght
        }

        


        private void curUpl_UploadStatusChanged ( object sender, UploadStatus e ) {
            if (e != UploadStatus.Uploaded && this.CurrentStatus != UploadStatus.Canceled) {
                this.CurrentStatus = e;
            }
        }


        public void Suspend ( ) {
            if (curUpl != null) {
                curUpl.Suspend();
            }
            else {
                this.CurrentStatus = UploadStatus.Canceled;
            }
        }


        public void Resume ( ) {
            if (curUpl != null) {
                curUpl.Resume();
            }
            else {
                this.CurrentStatus = UploadStatus.Canceled;
            }
        }


        public void Stop ( ) {
            if (curUpl != null) {
                curUpl.Stop();
            }
            else {
                this.CurrentStatus = UploadStatus.Canceled;
            }
        }


        public void Close ( ) {
            if (this.curUpl != null) {
                this.curUpl.Close();
            }
        }


        void IDisposable.Dispose ( ) {
            this.Close();
        }
    }
}