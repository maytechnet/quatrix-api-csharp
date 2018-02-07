using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maytech.Quatrix.Entity {


    /// <summary>
    /// Parameters for CreateShare method
    /// </summary>
    [Serializable]
    public class ShareParameters {


        /// <summary>
        /// Determines the type of share
        /// </summary>
        [Flags]
        public enum ShareType {
            /// <summary>
            /// Anyone can download. No tracking.
            /// </summary>
            Public = 0,
            /// <summary>
            /// Any registered user can download. Full tracking
            /// </summary>
            Tracked = 1,
            /// <summary>
            /// Only the registered email recipient(s) can download. Full tracking.
            /// </summary>
            Confidential = 2
        }

        private ShareType type;                         //share type
        private DateTime expire_date;                   //expire date
        private DateTime release_date;                  //release date


        internal readonly Profile profile;               //current user


        /// <summary>
        /// Gets or sets the ID of instant share object using which you want to share
        /// </summary>
        [JsonProperty]
        private string id {
            get {
                return profile.id;
            }
        }


        [JsonIgnore]
        public Metadata Folder { get; set; }


        /// <summary>
        /// Gets or sets the ID of the folder that contains files you want to share
        /// </summary>
        [JsonProperty]
        private string folder_id {
            get {
                return Folder.id;
            }
        }


        /// <summary>
        /// Gets or sets the list of file IDs you want to share
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Metadata> Files { get; set; }


        /// <summary>
        /// Gets or sets the list of file IDs you want to share
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> files { get; set; }


        /// <summary>
        /// Gets or sets the list of file IDs you want to share
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Contact> Contacts { get; set; }


        /// <summary>
        /// Gets or sets the list of recipient IDs that you want to share
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> contacts { get; set; }


        /// <summary>
        /// Gets or sets text of the email subject
        /// </summary>
        [JsonProperty]
        public string subject { get; set; }


        /// <summary>
        /// Gets or sets text of the email message
        /// </summary>
        [JsonProperty]
        public string message { get; set; }


        [JsonIgnore]
        public DateTime Expire {
            get {
                return this.expire_date;
            }
            set {
                expire_date = value;
                if( expire_date == DateTime.MinValue ) {
                    //Expire date not set
                    expires = 0;
                    return;
                }
                if( !IsDatesValid ) {
                    throw new QuatrixShareException( Messages.Error.api_share_invalid_expire, QuatrixShareException.INVALID_EXPIRE_DATE );
                }
                expires = (long)Utils.GetTimestamp( expire_date.ToUniversalTime() );
            }
        }


        /// <summary>
        /// Gets or sets UTC time when the sharing files/folders expire. if it is set as 0, it will never expire.
        /// </summary>
        [JsonProperty]
        public long expires { get; set; }


        [JsonIgnore]
        public DateTime Release {
            get {
                return release_date;
            }
            set {
                release_date = value;
                if( release_date == DateTime.MinValue ) {
                    //Expire date not set
                    activates = null;
                    return;
                }
                if( !IsDatesValid ) {
                    throw new QuatrixShareException( Messages.Error.api_share_invalid_release, QuatrixShareException.INVALID_RELEASE_DATE );
                }
                activates = (long)Utils.GetTimestamp( release_date.ToUniversalTime() );
            }
        }


        /// <summary>
        /// Gets or sets UTC time when the sharing files/folders are activated. 
        /// </summary>
        /// <value>
        /// If is set as 0, it will be instantly activated.
        /// </value>
        [JsonProperty]
        public long? activates { get; set; }


        /// <summary>
        /// Gets or sets the value indicates whether the user can return files.
        /// </summary>
        [JsonProperty]
        public bool return_files { get; set; }


        /// <summary>
        /// Gets or sets the value indicates whether files are PGP encrypted.
        /// </summary>
        [JsonProperty]
        public bool pgp_encrypted { get; set; }


        /// <summary>
        /// Gets or sets the value indicates whether returned files should be PGP encrypted. It can only be true, if 'return_files' is true
        /// </summary>
        [JsonProperty]
        public bool return_pgp_encrypted { get; set; }


        /// <summary>
        /// Gets or sets email signature text
        /// </summary>
        [JsonProperty]
        public string message_signature { get; set; }


        /// <summary>
        /// Gets or sets the value indicates whether notificate the sender if shared files/folders are downloaded.
        /// </summary>
        [JsonProperty]
        public bool notify { get; set; }


        /// <summary>
        /// Get;Set share type
        /// </summary>
        [JsonIgnore]
        public ShareType Type {
            get {
                return type;
            }
            set {
                type = value;
                switch( type ) {
                    case ShareType.Public: {
                        share_type = "P";
                        break;
                    }
                    case ShareType.Tracked: {
                        share_type = "T";
                        break;
                    }
                    case ShareType.Confidential: {
                        share_type = "C";
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets the type of share. USE SetShareType(ShareType) method to set correct value
        /// </summary>
        [JsonProperty]
        private string share_type { get; set; }


        /// <summary>
        /// Compares expire and release date
        /// </summary>
        [JsonIgnore]
        protected bool IsDatesValid {
            get {
                return !HasExpire || !HasRelease || activates < expires;
            }
        }


        [JsonIgnore]
        public bool HasExpire {
            get {
                return expires != 0;
            }
        }


        [JsonIgnore]
        public bool HasRelease {
            get {
                return activates != null && activates != 0;
            }
        }


        [JsonProperty]
        public string pin { get; set; }


        //ctor
        public ShareParameters( Profile profile ) {
            if( profile == null ) {
                throw new ArgumentNullException( "profile" );
            }
            this.profile = profile;
            Expire = DateTime.MinValue;
            Release = DateTime.MinValue;
            Type = ShareType.Public;
            pin = null;
        }


        /// <summary>
        /// Serialize this parameters to json string and then get bytes(UTF8)
        /// </summary>
        /// <returns></returns>
        internal object GetRequestData() {
            if( Folder == null ) {
                throw new ArgumentException( "Folder" );
            }
            //check if current metadata is valid
            if( Folder.Type != MetadataType.DIRECTORY && Folder.Type != MetadataType.USER_SHARED_FOLDER ) {
                throw new QuatrixMetadataNotDirectoryException();
            }
            //check if we have files for share
            //select ids
            files = Files.
                        Where( f => f != null ).
                        Select( f => f.id ).
                        ToArray();
            if( files == null || files.Count() == 0 ) {
                throw new QuatrixShareException( Messages.Error.api_share_no_files, QuatrixShareException.SHARE_WITHOUT_FILES );
            }
            contacts = Contacts.
                        Where( f => f != null ).
                        Select( f => f.id ).
                        ToArray();
            //check if receivers
            if( contacts == null || contacts.Count() == 0 ) {
                throw new QuatrixShareException( Messages.Error.api_share_no_receivers, QuatrixShareException.SHARE_WITHOUT_RECEIVERS );
            }
            if( !string.IsNullOrEmpty( pin ) ) {
                pin = Convert.ToBase64String( System.Text.Encoding.UTF8.GetBytes( pin.ToString() ) );
            }
            return this;
        }
    }
}