using System;


namespace Maytech.Quatrix {


    [Serializable]
    public sealed class UploadInfo : QEntity {


        internal UploadInfo() { }


        [Newtonsoft.Json.JsonProperty]
        public string file_id {
            get {
                return id;
            }
            internal set {
                id = value;
            }
        }


        /// <summary>
        /// Upload key (part of upload link)
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string upload_key { get; internal set; }


        /// <summary>
        /// Parent metadata id
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string parent_id { get; internal set; }


        /// <summary>
        /// Parent metadata entity
        /// </summary>
        public Metadata Parent { get; internal set; }


        /// <summary>
        /// Name of upload file (can be modified if in current metadata exist such name)
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string name { get; internal set; }


        /// <summary>
        /// Return upload link
        /// </summary>
        /// <returns></returns>
        public string GetUploadRoute() {
            //To form upload link following template should be used:
            //`/ upload /< upload_type >/< upload_key >`
            //upload_type can be like 'multipart' or 'chunked' 
            //in our case we use 'chunked'
            return string.Concat( QRequest.UPLOAD_CHUNKED, upload_key );
        }
    }
}
