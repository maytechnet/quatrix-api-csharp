using Maytech.Quatrix.Entity;

namespace Maytech.Quatrix {


    public sealed class ShareAction : QEntity {

        /*
            /is/list_links/{id} - get files download links
        */

        private const string DOWNLOAD = "/download/";
        private const string RETURN = "/files-return/";


        public ShareParameters Parameters { get; private set; }


        internal ShareAction( Id shareId, ShareParameters parameters ) {
            id = shareId.id;
            Request = shareId.Request;
            Parameters = parameters;
        }


        /// <summary>
        /// Creates the link for download shared files.
        /// </summary>
        public string GetDownloadLink() {
            return string.Concat( Request.Referer, DOWNLOAD, id );
        }


        public string GetReturnLink() {
            return string.Concat( Request.Referer, RETURN, id );
        }
    }
}

