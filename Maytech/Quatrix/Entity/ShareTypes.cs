namespace Maytech.Quatrix.Entity {


    [System.Serializable]
    public class ShareTypes {


        internal ShareTypes() { }


        [Newtonsoft.Json.JsonProperty]
        public bool tracked_share { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public bool public_share { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public bool restricted_share { get; internal set; }
    }
}
