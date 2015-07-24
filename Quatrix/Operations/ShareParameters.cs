using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Maytech.Quatrix {

    /// <summary>
    /// Parameters for method create share
    /// </summary>
    [System.Serializable]
    public class ShareParameters {

        [System.Flags]
        public enum ShareType {
            Public = 0,
            Tracked = 1,
            Confidential = 2
        }

        
        /// <summary>
        /// the ID of instant share object using which you want to share
        /// </summary>
        public string id {
            get;
            set;
        }


        /// <summary>
        /// the ID of the folder that contains files you want to share
        /// </summary>
        public string folder {
            get;
            set;
        }


        /// <summary>
        /// specifies the list of file IDs you want to share
        /// </summary>
        public IEnumerable<string> files {
            get;
            set;
        }


        /// <summary>
        /// specifies the list of recipient IDs that you want to share
        /// </summary>
        public IEnumerable<string> contacts {
            get;
            set;
        }


        /// <summary>
        /// text of the email subject
        /// </summary>
        public string subject {
            get;
            set;
        }


        /// <summary>
        /// text of the email message
        /// </summary>
        public string message {
            get;
            set;
        }


        /// <summary>
        /// contains UTC time when the sharing files/folders expire. if it is set as 0, it will never expire.
        /// </summary>
        public long expires {
            get;
            set;
        }


        /// <summary>
        /// specifies UTC time when the sharing files/folders are activated. If it is set as 0, it will be instantly activated.
        /// </summary>
        public long activates {
            get;
            set;
        }


        /// <summary>
        /// enables to return files.
        /// </summary>
        public bool return_files {
            get;
            set;
        }


        /// <summary>
        /// shows that files were PGP encrypted.
        /// </summary>
        public bool pgp_encrypted {
            get;
            set;
        }


        /// <summary>
        /// specifies that returned files should be PGP encrypted. It can only be true, if 'return_files' is true.
        /// </summary>
        public bool return_pgp_encrypted {
            get;
            set;
        }


        /// <summary>
        /// email signature text
        /// </summary>
        public string message_signature {
            get;
            set;
        }


        /// <summary>
        /// enables notifications which are sent after downloading files.
        /// </summary>
        public bool notify {
            get;
            set;
        }


        /// <summary>
        /// the type of share
        /// USE SetShareType for set correct value
        /// </summary>
        public string share_type {
            get;
            set;
        }



        /// <summary>
        /// Serialize ShareParameters to json type
        /// </summary>
        /// <returns>json parametes</returns>
        public string GetJsonParameters() {
            JToken t = GetJsonToken();
            return t == null ? string.Empty : t.ToString();
        }


        public bool InitMain ( Profile profile, Metadata folder, IEnumerable<Metadata> files, IEnumerable<Contact> contacts ) {
            //Check on correct params
            if (profile == null || folder == null || files == null || contacts == null) {
                return false;
            }
            this.id = profile.id;
            this.folder = folder.id;
            //Review files
            files = files.Where( f => f != null );
            if (files.Count() > 0) {
                this.files = files.Select( f => f.id );
            }
            else {
                return false;
            }
            //Review contacts
            contacts = contacts.Where( cn => cn != null );
            if (contacts.Count() > 0) {
                this.contacts = contacts.Select( c => c.id );
            }
            else {
                return false;
            }
            return true;
        }


        public bool isInited ( ) {
            return !(string.IsNullOrEmpty( this.id ) || string.IsNullOrEmpty( this.id ) || this.folder == null || this.contacts == null);
        }


        public JToken GetJsonToken ( ) {
            if (isInited()) {
                return JToken.FromObject( this );
            }
            else {
                return null;
            }
        }


        /// <summary>
        /// Set correct value for share_type
        /// </summary>
        /// <param name="share_type">flag of share type</param>
        public void SetShareType( ShareType share_type ) {
            switch (share_type) {
                case ShareType.Public: {
                    this.share_type = "P";
                    break;
                }
                case ShareType.Tracked: {
                    this.share_type = "T";
                    break;
                }
                case ShareType.Confidential: {
                    this.share_type = "C";
                    break;
                }
            }
        }


        public ShareType GetShareType ( ) {
            switch (share_type) {
                case "P": {
                    return ShareType.Public;
                }
                case "T": {
                    return ShareType.Tracked;
                }
                case "C": {
                    return ShareType.Confidential;
                }
                default: {
                    return ShareType.Tracked;
                }
            }
        }
    }
}
