namespace Maytech.Quatrix {


    /// <summary>
    /// Represents information about user Quota. This class can't be inherited
    /// </summary>
    [System.Serializable]
    public sealed class ProfileInfo : QEntity {


        internal ProfileInfo() { }


        /// <summary>
        /// Gets the used size.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public long user_used { get; internal set; }


        /// <summary>
        /// Gets the max size that user can use.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public long user_limit { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public long acc_used { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public long acc_limit { get; internal set; }
    }
}
