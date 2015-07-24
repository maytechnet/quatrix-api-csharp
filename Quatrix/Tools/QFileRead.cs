using Maytech.Quatrix.Messages;
using System;
using System.IO;

namespace Maytech.Quatrix.Tools {



    public abstract class QFileRead : System.IDisposable {

        private FileStream fileStream;
        protected int blockSize;
        protected long offset;


        public long Lenght {
            get {
                return fileStream.Length;
            }
        }


        public FileInfo CurrentFile {
            get;
            private set;
        }


        protected virtual bool MoveNext {
            get {
                return offset < fileStream.Length;
            }
        }


        public virtual long BytesUploaded {
            get {
                return offset;
            }
            protected set {
                offset = value;
            }
        }


        public QFileRead ( string path )
            : this( path, 4000000 ) {

        }


        public QFileRead ( string path, int blockSize ) {
            this.blockSize = blockSize;
            offset = 0;
            if (File.Exists( path )) {
                fileStream = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
                CurrentFile = new FileInfo( path );
            }
            else {
                throw new QException( Error.file_not_exist );
            }

            if (fileStream.Length < blockSize) {
                blockSize = (int)fileStream.Length;
            }
        }


        protected virtual byte[] GetBlock ( out int lenght ) {
            lenght = (int)( ( blockSize + offset < Lenght ) ? blockSize : Lenght - offset );
            byte[] buffer = new byte[lenght];
            fileStream.Seek( offset, SeekOrigin.Begin );
            fileStream.Read( buffer, 0, lenght );
            return buffer;
        }


        public void Close ( ) {
            if (fileStream != null) {
                fileStream.Close();
            }
        }


        void IDisposable.Dispose ( ) {
            Close();
        }
    }
}
