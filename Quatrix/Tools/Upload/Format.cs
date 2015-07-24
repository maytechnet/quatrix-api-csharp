using Maytech.Quatrix.Messages;

namespace Maytech.Quatrix.Tools.Upload {
    
    
    internal static class Format {


        public static string UploadStatusFormated ( this UploadStatus status ) {
            string c_status = string.Empty;
            switch (status) {
                case UploadStatus.Waiting: {
                    c_status = Info.status_waiting;
                    break;
                }
                case UploadStatus.Upload: {
                    c_status = Info.status_upload;
                    break;
                }
                case UploadStatus.Uploaded: {
                    c_status = Info.status_uploaded;
                    break;
                }
                case UploadStatus.Suspended: {
                    c_status = Info.status_suspend;
                    break;
                }
                case UploadStatus.Disconnected: {
                    c_status = Info.status_disconnected;
                    break;
                }
                case UploadStatus.Canceled: {
                    c_status = Info.status_canceled;
                    break;
                }
            }
            return string.Format( "{0}:{1}", Info.status_name, c_status );
        }
    }
}
