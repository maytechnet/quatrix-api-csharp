namespace Maytech.Quatrix {


    /// <summary>
    /// Basic class for  Quatrix Entities
    /// </summary>
    [System.Serializable]
    public abstract class QEntity : IQEntity {


        internal QEntity() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="QEntity"/> class  with specified request.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        public QEntity( IQuatrixRequest request ) {
            this.Request = request;
        }


        /// <summary>
        /// Gets the request.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public virtual IQuatrixRequest Request { get; internal set; }


        /// <summary>
        /// Gets entity identifier.
        /// </summary>
        [Newtonsoft.Json.JsonProperty]
        public virtual string id { get; internal set; }
    }
}
