using System;
using System.Runtime.Serialization;

namespace Maytech.Quatrix.Entity {


    [Serializable]
    public abstract class MetadataAttributes : QEntity, IQMetadata {

        public const int FILE_LINK_TYPE = 'N';
        public const int FILE_TYPE = 'F';
        public const int DIRECTORY_TYPE = 'D';
        public const int SHARED_TYPE = 'S';
        public const int TRASH_TYPE = 'T';


        internal MetadataAttributes() { }


        /// <summary>
        /// Gets metadata name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public string name { get; internal set; }


        /// <summary>
        /// Gets metadata parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        [Newtonsoft.Json.JsonProperty]
        public string parent_id { get; internal set; }


        public Metadata Parent { get; internal set; }


        /// <summary>
        /// Gets metadata type. 
        /// </summary>
        /// <remarks> There are two metadata types: "F" for files and "D"  for directories </remarks>
        [Newtonsoft.Json.JsonProperty]
        internal string type { get; set; }


        [Newtonsoft.Json.JsonIgnore]
        public MetadataType Type { get; internal set; }


        /// <summary>
        /// Gets metadata size.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public long size { get; internal set; }


        /// <summary>
        /// Gets metadata modified date.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public long modified { get; internal set; }


        /// <summary>
        /// Gets metadata expire date.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public long expires { get; internal set; }


        /// <summary>
        /// Get metadata operations.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string operations { get; internal set; }


        [Newtonsoft.Json.JsonProperty]
        public Description metadata { get; internal set; }


        public virtual void Refresh() {
            var upd = Operations.FileOperations.GetMetadata( Request, id, false );
            id = upd.id;
            modified = upd.modified;
            name = upd.name;
            operations = upd.operations;
            parent_id = upd.parent_id;
            expires = upd.expires;
            size = upd.size;
            type = upd.type;
            Type = upd.Type;
        }


        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            char result;
            if( char.TryParse( type, out result ) ) {
                Type = (MetadataType)result;
            }
            if( Type == MetadataType.DIRECTORY ) {
                if( metadata != null ) {
                    Type = string.Compare( metadata.subtype, Metadata.SHARED_PROJECTS_SUBTYPE, true ) == 0 ?
                        MetadataType.SYSTEM_SHARED_FOLDER : MetadataType.SYSTEM_DIRECTORY;
                }
            } else if( Type == MetadataType.SYSTEM_SHARED_FOLDER && metadata == null ) {
                Type = MetadataType.USER_SHARED_FOLDER;
            }
        }
    }
}