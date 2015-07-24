using Maytech.Quatrix.Tools;
using Maytech.Quatrix.Tools.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace Maytech.Quatrix.Operations {

    /// <summary>
    /// Quatrix Share Actions API provides functionality for sharing files/folders to other users.
    /// </summary>
    public static class ShareOperations {


        public static ShareAction CreateShareAction ( this IQEntity e, ShareParameters parameters ) {
            QEntity share = e.Request.MakeApiCall<ShareAction>( "/is/create_share", Encoding.UTF8.GetBytes( parameters.GetJsonParameters() ) );
            
            return new ShareAction {
                id = share.id,
                download = string.Format( "/download/{0}", share.id ),
                links = string.Format( "/is/list_links/{0}", share.id ),
                Request = e.Request
            };
        }


        public static string CreateShare ( this ShareAction action ) {
            if (!string.IsNullOrEmpty( action.id ) ) {
                string api_call = action.download;
                return action.Request.CreateHostUri( api_call );
            }
            else {
                return string.Empty;
            }
        }


        /// <summary>
        /// This API call is used to create a share
        /// </summary>
        /// <param name="api_object">Object which have request data</param>
        /// <param name="parameters">Share parameters</param>
        /// <returns>Result ID can be used to form download page link using following template: https://[hostname]/download/[id]</returns>
        public static string CreateShare ( this IQEntity api_object, ShareParameters parameters ) {
            if (!parameters.isInited()) {
                return string.Empty;
            }
            return CreateShare( CreateShareAction( api_object, parameters ) );
        }


        public static IEnumerable<ShareFile> GetShareFiles ( this ShareAction action ) {
            IQuatrixRequest request = action.Request;
            request.Enable();
            string json = request.CreateRequest( action.links );
            return JToken.Parse( json ).ToObject<List<ShareFile>>();
        }


        public static string GetDownloadLink ( this ShareAction action, string[] links ) {
            IQuatrixRequest request = action.Request;
            request.Enable();
            string api_call = string.Format("/is/get_download_link/{0}", action.id);
            string part = request.CreateRequest( api_call, 
                Parameters.ShareDownloadLink( action.id, links ) );
            return request.CreateApiUri( string.Format("/is/{0}", part) );
        }


        //public static IShareDownloadInfo GetShareDownloadInfo ( this Profile profile ) {
        //    IQuatrixRequest request = profile.Request;
        //    request.RequestEnable();
        //    //var info = profile.GetUserShareMetadata();
        //    string api_call = string.Format( "/is/download_info/{0}", info.id );
        //    return request.CreateRequest<ShareDownloadInfo>( request.CreateApiUri( api_call ) );
        //}
    }
}
