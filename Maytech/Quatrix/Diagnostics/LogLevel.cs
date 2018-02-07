using System;

namespace Maytech.Quatrix.Diagnostics {

    [Flags]
    public enum LogLevel {
        Fatal = 1,
        Error = 2,
        Info = 4,
        Debug = 8
    }
}
