using System;

namespace Maytech.Quatrix.Entity {


    [Serializable]
    public class Service : QEntity {


        [Newtonsoft.Json.JsonProperty]
        public string service_key { get; internal set; }
    }
}
