using System;

namespace Maytech.Quatrix.Transfer {


    public interface IQuatrixTransfer {


        /// <summary>
        /// Occurs when uploading is started.
        /// </summary>
        event EventHandler Started;


        /// <summary>
        /// Occurs when uploading is finished.
        /// </summary>
        event EventHandler Finished;


        /// <summary>
        /// Gets the full path of file.
        /// </summary>
        string FullName { get; }


        /// <summary>
        /// Begin transfer
        /// </summary>
        void Start();
    }
}
