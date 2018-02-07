using System;
using System.Collections.Generic;

namespace Maytech.Quatrix.Entity {

    [Serializable]
    public class Diff : QEntity {


        [Newtonsoft.Json.JsonProperty]
        internal List<DiffAction> data;


        [Newtonsoft.Json.JsonProperty]
        public long to { get; set; }


        [Newtonsoft.Json.JsonProperty]
        public long from { get; set; }


        public IEnumerable<DiffAction> Data {
            get {
                return data;
            }
        }


        internal Diff() {
            data = null;
        }
    }
}
