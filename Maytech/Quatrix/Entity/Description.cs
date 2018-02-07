using System;

namespace Maytech.Quatrix {


    [Serializable]
    public class Description {


        internal Description() { }


        [Newtonsoft.Json.JsonProperty]
        public int visual_order { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string subtype { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string translation_id { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public bool is_system { get; internal set; }
    }
}
