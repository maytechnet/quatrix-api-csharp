namespace Maytech.Quatrix.Entity {


    [System.Serializable]
    public class AccountInfo : QEntity {

        public const string SUSPENDED = "S";

        internal AccountInfo() { }

        /// <summary>
        /// Gets account status.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string status { get; internal set; }

        /// <summary>
        /// Gets account language.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string language { get; internal set; }

        /// <summary>
        /// Gets accounts title.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string title { get; internal set; }

        public bool IsSuspended {
            get {
                return status == SUSPENDED;
            }
        }
    }
}
