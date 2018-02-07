namespace Maytech.Quatrix {


    public interface IQuatrixRequest {


        /// <summary>
        /// Quatrix host account
        /// </summary>
        string Referer { get; }


        /// <summary>
        /// Base api Uri
        /// </summary>
        string ApiUri { get; }


        /// <summary>
        /// Key for using authorized api calls
        /// </summary>
        string Token { get; }


        /// <summary>
        /// Create quatrix authorized http get request
        /// </summary>
        /// <param name="route">One of api calls described in Maytech public docs.</param>
        /// <returns>String representation of server json response</returns>
        string Get( string route );


        /// <summary>
        /// Create quatrix authorized http get request
        /// </summary>
        /// <param name="route">One of api calls described in Maytech public docs.</param>
        /// <returns>deserialized object</returns>
        T Get<T>( string route ) where T : QEntity;


        /// <summary>
        /// Create quatrix authorized http get request
        /// </summary>
        /// <param name="route">One of api calls described in Maytech public docs.</param>
        /// <returns>String representation of server json response</returns>
        string Post( string route, object data );


        /// <summary>
        /// Create quatrix authorized http get request
        /// </summary>
        /// <param name="route">One of api calls described in Maytech public docs.</param>
        /// <returns>deserialized object</returns>
        T Post<T>( string route, object data ) where T : QEntity;
    }
}
