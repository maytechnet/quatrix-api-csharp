
namespace Maytech.Quatrix {


    [System.Serializable]
    public sealed class UserPgpInfo : QEntity {


        internal UserPgpInfo ( ) {
        
        }


        [Newtonsoft.Json.JsonProperty]
        public string name {
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
        public string @public {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string @private {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string user {
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
