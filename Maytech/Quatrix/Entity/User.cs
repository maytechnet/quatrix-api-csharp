namespace Maytech.Quatrix {
    
    
    /// <summary>
    /// Represents information about Quatrix user. This class can't be inherited.
    /// </summary>
    [System.Serializable]
    public sealed class User : QEntity {


        internal User() { }


        /// <summary>
        /// Gets users parent ID
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string parent { get; internal set; }


        /// <summary>
        /// Gets users real name.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string realname { get; internal set; }

        /// <summary>
        /// Gets users home folder id.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string home { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string home_name { get; internal set; }


        /// <summary>
        /// Gets value indicating if profile has administrator permissions.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string super_admin { get; internal set; }


        /// <summary>
        ///  Gets user file usage quota
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public long quota { get; internal set; }


        /// <summary>
        /// Gets user status
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string status { get; internal set; }


        /// <summary>
        /// Gets users email.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string email { get; internal set; }


        /// <summary>
        /// Gets user expiration time after which user will not be able to log in
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int expires { get; internal set; }


        /// <summary>
        /// Gets time when user was created.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int created { get; internal set; }


        /// <summary>
        /// Gets time when user was created.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int modified { get; internal set; }


        /// <summary>
        /// Gets user permissions
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int user_operations { get; internal set; }


        /// <summary>
        /// Gets user home folder permissions
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int home_operations { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string instant_share { get; internal set; }


        /// <summary>
        /// Gets users language.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string language { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public bool has_key { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public long usage { get; internal set; }
    }
}
