
namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class ShareDownloadInfo : QEntity {


        internal ShareDownloadInfo ( ) {
        
        }


        [Newtonsoft.Json.JsonProperty]
        public string message {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string subject {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int activates {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int expires {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string sender_name {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string sender_email {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public bool pgp_encrypted {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string private_key {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public bool enable_pgp {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string status {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string share_type {
            get;
            internal set;
        }
    }
}
