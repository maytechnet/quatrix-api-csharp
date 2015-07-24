//using Maytech.ApiObjects;
using Maytech.Quatrix.Tools.Net;

namespace Maytech.Quatrix.Operations {

    /// <summary>
    /// The following API group provides a possibility to manage user profile.
    /// </summary>
    public static class ProfileActions {

        /// <summary>
        /// This API call returns information about a user profile.
        /// </summary>
        /// <param name="request">request of api</param>
        /// <returns>User profile</returns>
        public static Profile GetProfile ( this IQuatrixRequest r ) {
            return r.MakeApiCall<Profile>( "/profile/get" );
        }
    }
}
