using System;
using Maytech.Quatrix;
using Maytech.Quatrix.Transfer;

namespace Maytech.Demo
{
    /// <summary>
    /// This class is used to upload file into the cloud
    /// </summary>
    class FileUpload:FileChunkUpload
    {

        /// <param name="path">file path</param>
        /// <param name="session">request to send </param>
        /// <param name="parentId">id of the folder in what folder in cloud this file will be uploaded</param>
        public FileUpload(string path,IQuatrixRequest session,string parentId):base(path,Settings.DEFAULT_CHUNK_SIZE)
        {
            ResolveName = false;
            Session = session;
            ParentId = parentId;
        }
    }
}
