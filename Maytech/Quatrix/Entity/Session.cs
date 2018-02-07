namespace Maytech.Quatrix {


    [System.Serializable]
    public sealed class Session : QEntity {


        internal Session() { }


        /// <summary>
        /// Gets the session uuid.
        /// </summary>
        /// <value>
        /// The account id.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public string session_id {
            get {
                return id;
            }
            internal set {
                id = value;
            }
        }
    }
}
