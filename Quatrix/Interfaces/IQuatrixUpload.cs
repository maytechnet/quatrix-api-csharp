using Maytech.Quatrix.Tools;
using Maytech.Quatrix.Tools.Upload;
using System;

namespace Maytech.Quatrix {
    
    public interface IQuatrixUpload {

        event ChunkedUploadProgress UploadProgressChanged;

        event ChunkedUploadStatusChanged UploadStatusChanged;

        event EventHandler UploadStarted;

        event EventHandler UploadFinished;


        string SpecifiedPath {
            get;
        }


        string SpecifiedName {
            get;
            set;
        }


        UploadStatus CurrentStatus {
            get;
        }


        Metadata Parent {
            get;
            set;
        }


        Metadata Result {
            get;
        }


        long Length {
            get;
        }


        string CurrentStatusFormated {
            get;
        }


        long BytesUploaded {
            get;
        }


        void Start ( );


        void Suspend ( );


        void Resume ( );


        void Stop ( );


        void Close ( );
    }
}
