
namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class Contact : QEntity {

        internal Contact ( ) {

        }


        [Newtonsoft.Json.JsonProperty]
        public bool has_key {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string email {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public int created {
            get;
            internal set;
        }



        [Newtonsoft.Json.JsonProperty]
        public string name {
            get;
            internal set;
        }



        [Newtonsoft.Json.JsonProperty]
        public string status {
            get;
            internal set;
        }
    }
}
