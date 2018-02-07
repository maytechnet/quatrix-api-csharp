namespace Maytech.Quatrix {

    /// <summary>
    /// Represents information about Quatrix entity
    /// </summary>
    public interface IQEntity {


        /// <summary>
        /// Gets the request.
        /// </summary>
        IQuatrixRequest Request { get; }


        /// <summary>
        /// Gets entity identifier.
        /// </summary>
        string id { get; }
    }
}
