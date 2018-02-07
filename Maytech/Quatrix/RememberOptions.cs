using System;

namespace Maytech.Quatrix {

    /// <summary>
    /// Determine data saving  options
    /// </summary>
    [Flags]
    public enum RememberOptions {
        /// <summary>
        /// Remember only host property
        /// </summary>
        Host = 0,
        /// <summary>
        /// Save only host name and login
        /// </summary>
        HostLogin = 1,
        /// <summary>
        /// Save all user credentials (host, login, passwor)
        /// </summary>
        All = 2
    }
}
