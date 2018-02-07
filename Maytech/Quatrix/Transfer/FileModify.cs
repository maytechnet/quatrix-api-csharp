namespace Maytech.Quatrix.Transfer {


    public class FileModify : FileChunkUpload {


        public string FileId { get; protected set; }


        protected FileModify( string fullName, int chunkSize ) : base( fullName, chunkSize ) {
            ParentId = "-";
        }


        protected override UploadInfo GetUploadInfo() {
            //Create object which serialize to json type
            object u = new {
                truncate = CurrentFile.Length
            };
            string route = string.Concat( QRequest.UPLOAD_MODIFY, FileId );     //api call for modification
            return Session.Post<UploadInfo>( route, u );                        //get upload info
        }
    }
}
