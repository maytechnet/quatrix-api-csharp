
namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class Profile : QEntity {

        internal Profile ( ) {

        }


        [Newtonsoft.Json.JsonProperty]
        public string operations {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string rootId {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string realname {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string image {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public bool is_contact {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public bool enable_pgp {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public bool has_key {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string plan {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int files_per_share {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public long max_file_size {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string language {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string channel_id {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string super_admin {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string email {
            get;
            internal set;
        }
    }
}
