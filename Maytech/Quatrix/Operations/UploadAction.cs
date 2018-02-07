namespace Maytech.Quatrix.Operations {


    /// <summary>
    /// Provides a possibility to upload files.
    ///  </summary>
    ///  <remarks>
    ///There are two types of uploads: multipart and chunked.
    ///Multipart upload uses a standard HTTP content type format of multipart/form-data.
    ///Chunked uploads are performed using 'Range' headers. The chunked upload requires explicit finalize action.
    ///The response of file content POST call contains ID of the uploaded file.
    ///</remarks>
    public static class UploadAction {


        /// <summary>
        /// Determines whether there is free space on user account
        /// </summary>
        /// <param name="request">HTTP web request</param>
        /// <param name="data_Length">Size of the file.</param>
        /// <returns>True is enough. Otherwise - false</returns>
        public static bool IsEnoughtSpace( this IQuatrixRequest request, long data_Length ) {
            ProfileInfo qi = request.GetProfileInfo();
            //quota.user_limit = -1 means that no user quota is set and account quota is used
            if( qi.user_limit == -1 ) {
                return qi.acc_used + data_Length <= qi.acc_limit;
            }
            return qi.user_used + data_Length <= qi.user_limit;
        }
    }
}