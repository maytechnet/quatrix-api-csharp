using Maytech.Quatrix.Tools;
//using Maytech.ApiObjects;
using Maytech.Quatrix.Tools.Net;
using Maytech.Quatrix.Tools.Upload;
using System.IO;


namespace Maytech.Quatrix.Operations {


    /// <summary>
    /// The following API group provides a possibility to upload files.
    ///There are two types of uploads: multipart and chunked.
    ///Multipart upload uses a standard HTTP content type format of multipart/form-data.
    ///Chunked uploads are performed using 'Range' headers. The chunked upload requires explicit finalize action.
    ///The response of file content POST call contains ID of the uploaded file.
    /// </summary>
    public static class UploadAction {

        /// <summary>
        /// The following API call provides a possibility to uploading files
        /// </summary>
        /// <param name="metadata">Root folder</param>
        /// <param name="path">Local file path</param>
        /// <returns>Uploaded metadata</returns>
        public static Metadata Upload ( this Metadata parent, string path ) {
            return Upload( parent, path, string.Empty );
        }


        public static Metadata Upload ( this Metadata parent, string path, string specifiedName ) {
            return Upload( parent, UploadFactory.GetUploader( path ), specifiedName );
        }

        /// <summary>
        /// The following API call provides a possibility to uploading files
        /// </summary>
        /// <param name="metadata">Root folder</param>
        /// <param name="upload">This parameter use if you want to set event or get properties of uploader. Close filestream after upload. Must be not null</param>
        /// <returns>Uploaded metadata</returns>
        public static Metadata Upload ( this Metadata parent, IQuatrixUpload upload ) {
            return Upload( parent, upload, upload.SpecifiedName );
        }


        public static Metadata Upload ( this Metadata parent, IQuatrixUpload upload, string specifiedName ) {
            upload.Parent = parent;
            upload.SpecifiedName = specifiedName;
            upload.Start();
            upload.Close();
            return upload.Result;
        }


        /// <summary>
        /// The following API call provides a possibility to get the link for uploading files
        /// This is chunke upload type
        /// </summary>
        /// <param name="metadata">parent metadata with ID of the folder where the file should be uploaded</param>
        /// <param name="path">local file path</param>
        /// <param name="uuid">id of file which upload</param>
        /// <param name="fileName">name of file</param>
        /// <returns>Upload URI</returns>
        internal static string GetUploadUri ( Metadata metadata, string path, out string uuid, ref string fileName ) {
            IQuatrixRequest request = metadata.Request;                                             //check request data
            request.Enable();
            fileName = string.IsNullOrEmpty( fileName ) ? Path.GetFileName( path ) : fileName;
            //Create POST request to get upload link
            string api_call = "/upload/getlink";
            string json = request.CreateRequest( api_call, Parameters.Upload( metadata.id, fileName ) );
            //Get uri link
            string uri = json.Trim( ' ', '"' );
            uuid = uri.Substring( uri.LastIndexOf( '/' ) + 1 );
            return uri;
        }


        /// <summary>
        /// The following API call finalizes a chunked upload.The ID is the uuid part of the upload link.
        /// </summary>
        /// <param name="request">Request that used in Getupload uri</param>
        /// <param name="uuid">upload file id</param>
        /// <param name="fileName">file name</param>
        /// <returns>Uploaded metadata</returns>
        internal static Metadata Finilize ( IQuatrixRequest request, string uuid, string fileName ) {
            string api_call = string.Format( "/upload/finalize/{0}", uuid );
            //Create get request to finilize upload
            Metadata result = request.CreateRequest<Metadata>( api_call );
            return result;
        }
    }
}
