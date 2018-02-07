namespace Maytech.Quatrix.Entity {

    [System.Serializable]
    public class Job : QEntity {


        internal Job() { }


        [Newtonsoft.Json.JsonProperty]
        public string job_id {
            get {
                return base.id;
            }
            internal set {
                base.id = value;
            }
        }
    }
}
