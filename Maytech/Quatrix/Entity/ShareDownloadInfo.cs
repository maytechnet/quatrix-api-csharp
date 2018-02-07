namespace Maytech.Quatrix {


    /// <summary>
    /// Represents information about related to action object. This class can't be inherited
    /// </summary>
    [System.Serializable]
    public sealed class ShareDownloadInfo : QEntity {


        internal ShareDownloadInfo() { }


        /// <summary>
        /// Gets the additional share message.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string message { get; internal set; }


        /// <summary>
        /// Gets share subject.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string subject { get; internal set; }


        /// <summary>
        /// Gets share release date
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int activates { get; internal set; }


        /// <summary>
        /// Gets share expire date.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int expires { get; internal set; }


        /// <summary>
        /// Gets the senders name.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string sender_name { get; internal set; }


        /// <summary>
        /// Gets the senders email.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string sender_email { get; internal set; }


        /// <summary>
        /// Gets a value indicating whether if files were PGP encrypted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if PGP encrypted; otherwise, <c>false</c>.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public bool pgp_encrypted { get; internal set; }


        /// <summary>
        /// Gets receiver's private PGP key (password protected)
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string private_key { get; internal set; }


        /// <summary>
        /// Gets a value indicating whether if PGP is enabled account-wise
        /// </summary>
        /// <value>
        ///   <c>true</c> if enable_pgp; otherwise, <c>false</c>.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public bool enable_pgp { get; internal set; }


        /// <summary>
        /// Gets share status.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string status { get; internal set; }


        /// <summary>
        /// Gets the type of share.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string share_type { get; internal set; }


        //public bool pgp_enabled { get; set; }
        //public string status { get; set; }
        //public object private_key { get; set; }
        //public string user_id { get; set; }
        //public string share_type { get; set; }
        //public string title { get; set; }
        //public string sender_name { get; set; }
        //public int expires { get; set; }
        //public bool pgp_encrypted { get; set; }
        //public string locale { get; set; }
        //public bool is_reply { get; set; }
        //public string sender_email { get; set; }
        //public string message { get; set; }
        //public int activates { get; set; }
        //public object user_name { get; set; }
        //public string id { get; set; }
        //public object user_email { get; set; }
        //public string subject { get; set; }
    }
}
