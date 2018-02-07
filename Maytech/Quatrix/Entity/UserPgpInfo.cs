
namespace Maytech.Quatrix {

    /// <summary>
    ///  Represents information about users PGP keys. This class can't be inherited.
    /// </summary>
    [System.Serializable]
    public sealed class UserPgpInfo : QEntity {


        internal UserPgpInfo() { }


        /// <summary>
        /// Gets PGP key name.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string name { get; internal set; }


        /// <summary>
        /// Gets the time when key was created.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int created { get; internal set; }


        /// <summary>
        /// Gets the time when key was modified.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int modified { get; internal set; }


        /// <summary>
        /// Gets the public part of PGP key.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string @public { get; internal set; }


        /// <summary>
        /// Gets the private part of PGP key.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string @private { get; internal set; }


        /// <summary>
        /// Gets key owners ID.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string user { get; internal set; }


        /// <summary>
        /// Gets key owners email.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string email { get; internal set; }
    }
}
