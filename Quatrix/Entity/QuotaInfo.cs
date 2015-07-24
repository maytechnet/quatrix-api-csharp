
namespace Maytech.Quatrix {
    

    [System.Serializable]
    public sealed class QuotaInfo : QEntity {

        internal QuotaInfo ( ) {

        }


        [Newtonsoft.Json.JsonProperty]
        public long user_used {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public long user_limit {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public long acc_used {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public long acc_limit {
            get;
            internal set;
        }
    }
}
