namespace Maytech.Quatrix.Entity {


    internal class QIds : QEntity {


        [Newtonsoft.Json.JsonProperty]
        public string[] Ids { get; internal set; }


        public QIds() {
            Ids = new string[0];
        }
    }
}
