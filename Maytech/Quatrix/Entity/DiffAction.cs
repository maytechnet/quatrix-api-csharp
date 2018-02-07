using System;

namespace Maytech.Quatrix.Entity {


    /// <summary>
    /// Represent single action from diff api call. Derived from <seealso cref="MetadataAttributes"/>
    /// </summary>
    [Serializable]
    public sealed class DiffAction : MetadataAttributes {

        public void SetData(string action, Metadata meta)
        {
            this.action = action;
            id = meta.id;
            expires = meta.expires;
            modified = meta.modified;
            name = meta.name;
            operations = meta.operations;
            Parent = meta.Parent;
            parent_id = meta.parent_id;
            size = meta.size;
            Request = meta.Request;
            Type = meta.Type;
            type = meta.type;

        }

        public void SetName(string newName)
        {
            name = newName;
        }

      

        [Newtonsoft.Json.JsonProperty]
        public string action { get; internal set; }
    }
}
