using Maytech.Quatrix.Entity;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace Maytech.Quatrix.Operations {


    /// <summary>
    /// Provides functionality for sharing files/folders to other users.
    /// </summary>
    public static class ShareOperations {


        /// <summary>
        /// Creates the share action.
        /// </summary>
        /// <param name="e">Web request for share action creating.</param>
        /// <param name="parameters">Share parameters.</param>
        /// <returns>New instance of <see cref="ShareAction"/>  class which contains information about new share action</returns>
        public static ShareAction CreateShareAction( this IQuatrixRequest request, ShareParameters parameters ) {
            Id shareId = request.Post<Id>( QRequest.SHARE_CREATE, parameters.GetRequestData() );
            return new ShareAction( shareId, parameters );
        }


        public static ShareDownloadInfo GetShareDownloadInfo( this IQuatrixRequest request, string shareId ) {
            return request.Get<ShareDownloadInfo>( string.Concat( QRequest.SHARE_DOWNLOAD_INFO, shareId ) );
        }


        //public static IEnumerable<ShareFile> GetShareFiles( this IQuatrixRequest request, string shareId ) {
        //    string json = request.Get( string.Concat( QRequest.SHARE_FILES, shareId ) );
        //    List<ShareFile> share_files = Utils.ToEntity<List<ShareFile>>( json );
        //    share_files.ForEach( ( f ) => f.Request = request );
        //    return share_files;
        //}


        public static Job SendShare( this ShareParameters parameters ) {
            string api_call = "/share/send";
            IQuatrixRequest request = parameters.profile.Request;
            return request.Post<Job>( api_call, parameters.GetRequestData() );
        }
    }
}