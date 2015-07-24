using System;

namespace Maytech.Quatrix.Tools.Upload {

    public class UploadEventArgs : EventArgs {

        internal long sentTicks;


        public UploadEventArgs( long bytesUploaded, long fileSize, int blockSize, long sentTicks ) {
            this.BytesUploaded = bytesUploaded;
            this.FileSize = fileSize;
            this.BlockSize = blockSize;
            this.sentTicks = sentTicks;
        }


        public long UploadSpeed {
            get {
                //long minute = 600000000; //ticks in minute
                long second = 10000000;
                return Convert.ToInt64( ( (double)second / sentTicks ) * BlockSize );
            }
        }


        public string UploadSpeedFormated {
            get {
                return UploadSpeed.FormatFileSize();
            }
        }


        public int BlockSize {
            get;
            private set;
        }


        public int Percent {
            get {
                return (int)(( BytesUploaded * 100 ) / FileSize);
            }
        }
        

        public long FileSize {
            get;
            private set;
        }


        public string FormatedFileSize {
            get {
                return FileSize.FormatFileSize();
            }
        }


        public long BytesUploaded {
            get;
            private set;
        }


        public string FormatedBytesUploaded {
            get {
                return BytesUploaded.FormatFileSize();  
            }
        }


        public override string ToString() {
            return Percent.ToString();
        }

    }
}
