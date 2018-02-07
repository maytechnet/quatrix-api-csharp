namespace Maytech.Quatrix {


    /// <summary>
    /// Represents information about Qatrix contact.This.class can't be inherited
    /// </summary>
    [System.Serializable]
    public sealed class Contact : QEntity {


        internal Contact() { }


        /// <summary>
        /// Gets a value indicating whether this <see cref="Contact"/>  has PGP key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if has key; otherwise, <c>false</c>.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public bool has_key { get; internal set; }


        /// <summary>
        /// Gets contacts email.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string email { get; internal set; }


        /// <summary>
        /// Gets date when contact was created .
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int created { get; internal set; }


        /// <summary>
        /// Gets contacts name.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string name { get; internal set; }


        /// <summary>
        /// Gets contacts status.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string status { get; internal set; }
    }
}
