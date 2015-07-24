
namespace Maytech.Quatrix {

    [System.Serializable]
    public sealed class ShareAction : QEntity {

        internal ShareAction ( ) {

        }


        [Newtonsoft.Json.JsonProperty]
        public string download {
            get;
            internal set;
        }


        [Newtonsoft.Json.JsonProperty]
        public string links {
            get;
            internal set;
        }
    }
}
