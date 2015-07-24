
namespace Maytech.Quatrix {
    
    [System.Serializable]
    public sealed class User : QEntity {

        internal User ( ) {
        
        }


        [Newtonsoft.Json.JsonProperty]
        public string parent {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string realname {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string home {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string home_name {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string super_admin {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int quota {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string status {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string email {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int expires {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int created {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int modified {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int user_operations {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int home_operations {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string instant_share {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string language {
            get;
            internal set;
        }
    }
}
