using Maytech.Quatrix.Entity;
using System.Collections.Generic;

namespace Maytech.Quatrix {


    /// <summary>
    /// Represents information about user profile.This class can't be inherited
    /// </summary>
    [System.Serializable]
    public sealed class Profile : QEntity {

        [Newtonsoft.Json.JsonProperty]
        private List<Service> services;


        internal Profile() {
            this.services = new List<Service>();
        }


        /// <summary>
        /// Gets profile  operations.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int operations { get; internal set; }


        /// <summary>
        /// Gets a value indicating whether PGP for  this <see cref="Profile"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public bool pgp_enabled { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public int? uid { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string incoming_id { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public int modified { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string outgoing_id { get; internal set; }


        /// <summary>
        /// Gets a value indicating whether this <see cref="Profile"/> is a contact.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is_contact; otherwise, <c>false</c>.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public bool is_contact { get; internal set; }


        /// <summary>
        /// Gets a value indicating whether this <see cref="Profile"/>  has PGP key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if has key; otherwise, <c>false</c>.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public bool has_key { get; internal set; }


        /// <summary>
        /// Gets the max size of uploading file
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public long? max_file_size { get; internal set; }


        /// <summary>
        /// Gets the root folder identifier.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string home_id { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public int? gid { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string storage_id { get; internal set; }


        /// <summary>
        /// Gets profiles email.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string email { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string status { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string account_id { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string message_signature { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public long quota { get; internal set; }


        /// <summary>
        /// Gets users profile plan.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string plan { get; internal set; }


        public List<Service> Services {
            get {
                return this.services;
            }
        }


        [Newtonsoft.Json.JsonProperty]
        public object unique_login { get; internal set; }


        /// <summary>
        /// Gets user name.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string name { get; internal set; }


        /// <summary>
        /// Gets profiles localization.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string language { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public int created { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public int? max_files_per_share { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string channel_id { get; internal set; }


        /// <summary>
        /// Gets value indicating if profile has administrator permissions.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string super_admin { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public ShareTypes share_types { get; internal set; }
    }
}
