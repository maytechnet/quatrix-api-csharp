using System.Collections.Generic;

namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class Metadata : QEntity {

        internal Metadata ( ) {
        
        }


        [Newtonsoft.Json.JsonProperty]
        public string name {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string parent {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string created {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string expires {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string modified {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string rel_type {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string shared {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string type {
            get;
            internal set;
        }



        [Newtonsoft.Json.JsonProperty]
        public string size {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string operations {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public IEnumerable<Metadata> ChildMetadata {
            get;
            internal set;
        }
    }
}
