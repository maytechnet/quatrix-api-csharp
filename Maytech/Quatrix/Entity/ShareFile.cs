namespace Maytech.Quatrix {


    /// <summary>
    /// Represents information about file(s), prepared to share. This class can't be inherited.
    /// </summary>
    [System.Serializable]
    public sealed class ShareFile : QEntity {


        internal ShareFile() { }


        /// <summary>
        /// Gets count of files 
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int count { get; internal set; }


        /// <summary>
        /// Gets file type. ("F" for file, "D" for directory)
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string type { get; internal set; }


        /// <summary>
        /// Gets file name.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public string name { get; internal set; }


        /// <summary>
        /// Gets the file size.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public int size { get; internal set; }        
    }
}
