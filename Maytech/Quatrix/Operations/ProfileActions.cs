namespace Maytech.Quatrix.Operations {


    /// <summary>
    /// The following API group provides a possibility to manage user profile.
    /// </summary>
    public static class ProfileActions {


        /// <summary>
        /// Returns information about a user profile
        /// </summary>
        /// <param name="r">HTTP request</param>
        /// <returns>Information about users profile</returns>
        public static Profile GetProfile( this IQuatrixRequest r ) {
            string api_call = "/profile";
            return r.Get<Profile>( api_call );
        }


        /// <summary>
        /// Get additional profile info
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>Profile info</returns>
        public static ProfileInfo GetProfileInfo( this IQuatrixRequest request ) {
            string api_call = string.Concat( QRequest.PROFILE_INFO );
            return request.Get<ProfileInfo>( api_call );
        }
    }
}
