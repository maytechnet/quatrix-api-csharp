using System.Collections.Generic;

namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class UserShareMetadata : QEntity {


        internal UserShareMetadata ( ) {

        }


        [Newtonsoft.Json.JsonProperty]
        public string name {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string language {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string message_footer {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string password_protected {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string incoming_folder_id {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string folder_id {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string email_footer {
            get;
            internal set;
        }



        [Newtonsoft.Json.JsonProperty]
        public IEnumerable<object> bcc {
            get;
            internal set;
        }



        [Newtonsoft.Json.JsonProperty]
        public string owner_id {
            get;
            internal set;
        }
    }
}
