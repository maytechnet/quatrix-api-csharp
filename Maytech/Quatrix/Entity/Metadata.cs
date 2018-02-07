using Maytech.Quatrix.Entity;
using System.Collections.Generic;

namespace Maytech.Quatrix {

    /// <summary>
    /// Represents information about files and folders on Quatrix cloud
    /// </summary>
    [System.Serializable]
    public sealed class Metadata : MetadataAttributes {


        public const string INBOX_SUBTYPE = "incoming";
        public const string OUTBOX_SUBTYPE = "outgoing";
        public const string USER_HOMES_SUBTYPE = "homes";
        public const string SHARED_PROJECTS_SUBTYPE = "projects";


        [Newtonsoft.Json.JsonProperty]
        internal Metadata[] content;


        internal Metadata() {
            content = new Metadata[0];
        }


        /// <summary>
        /// Gets metadata date created timestamp.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public long created { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string rel_type { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public string shared { get; internal set; }


        /// <summary>
        /// Gets list of child metadata.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public IEnumerable<Metadata> Content {
            get {
                return this.content;
            }
        }
        

        public override void Refresh() {
            var upd = Operations.FileOperations.GetMetadata( Request, id, Type == MetadataType.DIRECTORY );
            id = upd.id;
            content = upd.content;
            modified = upd.modified;
            name = upd.name;
            operations = upd.operations;
            parent_id = upd.parent_id;
            rel_type = upd.rel_type;
            created = upd.created;
            expires = upd.expires;
            size = upd.size;
            type = upd.type;
            Type = upd.Type;
        }
    }
}
