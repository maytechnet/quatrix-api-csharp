using System;

namespace Maytech.Quatrix.Tools.Upload {

    [Flags]
    public enum UploadStatus {
        Waiting = 0,
        Canceled = 1,
        Uploaded = 2,
        Upload = 3,
        Suspended = 4,
        Disconnected = 5
    }
}
