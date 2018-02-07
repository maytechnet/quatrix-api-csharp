using System;
using Maytech.Quatrix.Entity;
using Maytech.Quatrix.Transfer;
using Maytech.Quatrix;

namespace Maytech.Demo
{
    /// <summary>
    /// This class is used to upload folder to the cloud. It inherits from generic class FolderUpload with class FileUpload to upload content within folder
    /// </summary>
    class FolderUpload : FolderUpload<FileUpload>
    {

        public string Name { get; set; }
        
        /// <param name="folderName">target folder name</param>
        /// <param name="folderFullName">folder absolute path</param>
        /// <param name="quatrixRequest">request to upload file</param>
        /// <param name="parentId">parent id of library on the cloud in which you will place your folder</param>
        public FolderUpload(string folderName, string folderFullName, IQuatrixRequest quatrixRequest,string parentId) : base(folderFullName, Settings.DEFAULT_CHUNK_SIZE)
        {
            Recursive = true;
            ResolveName = false;
            Name = folderName;
            Session = quatrixRequest;
            ParentId = parentId;
        }
    }
}
