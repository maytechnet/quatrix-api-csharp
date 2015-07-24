
namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class ShareFile : QEntity {


        internal ShareFile ( ) {
        
        }


        [Newtonsoft.Json.JsonProperty]
        public string file_name {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string link {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int file_size {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int file_count {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string type {
            get;
            internal set;
        }
    }
}
