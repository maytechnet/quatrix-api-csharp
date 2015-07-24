using Maytech.Quatrix.Tools;
using Maytech.Quatrix.Tools.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Maytech.Quatrix.Operations {

    internal delegate Contact ContactOccuringHandler ( );

    /// <summary>
    /// The following API group provides a possibility to manage contacts.
    /// </summary>
    public static class ContactActions {


        /// <summary>
        /// This API call is used to create contact or get from contact list
        /// </summary>
        /// <param name="profile">User info</param>
        /// <param name="email">email of the contact (required)</param>
        /// <param name="name">name of the contac (optional)</param>
        /// <returns>New or Exist contact if email is valid, null if email is invalid</returns>
        public static Contact CreateOrGetContact ( this Profile profile, string email, string name ) {
            return profile.ContactOccuring( email, name, new ContactOccuringHandler( ( ) => {
                //chech if contact already exist
                return profile.Request.
                    GetContacts().
                    FirstOrDefault( c => c.email == email);
            } ) );
        }


        /// <summary>
        /// This API call is used to create contact
        /// </summary>
        /// <param name="profile">User info</param>
        /// <param name="email">email of the contact (required)</param>
        /// <param name="name">name of the contac (optional)</param>
        /// <returns>New or Exist contact if email is valid, null if email is invalid</returns>
        public static Contact CreateContact ( this Profile profile, string email, string name ) {
            return profile.ContactOccuring( email, name, new ContactOccuringHandler( ( ) => {
                return null;
            } ) );
        }


        /// <summary>
        /// "Create contact" occuring
        /// </summary>
        /// <param name="profile">User info</param>
        /// <param name="email">email of the contact (required)</param>
        /// <param name="name">name of the contac (optional)</param>
        /// <param name="before">Delegate which call before api request</param>
        /// <returns></returns>
        private static Contact ContactOccuring ( this Profile profile, string email, string name, ContactOccuringHandler before ) {
            IQuatrixRequest request = profile.Request;
            request.Enable();                                            //check if request not null
            Contact result = null;
            if (email.IsValidEmail()) {                                         //check for email validation
                //check if user try to create contact with own email
                if (email == profile.email) {
                    return new Contact() {                                      //return user data as contact
                        id = profile.id,
                        email = profile.email,
                        Request = profile.Request,
                        name = profile.realname,
                        has_key = profile.has_key
                    };
                }

                if (( result = before() ) != null) {
                    return result;
                }

                name = name.Trim();
                //Check if name parameter not set
                //Name will created from email address
                if (string.IsNullOrEmpty( name )) {
                    int at = email.IndexOf( '@' );
                    name = email.Substring( 0, at );
                    while (name.Length < 3) {
                        name = string.Format( "{0}_", name );
                    }
                }

                string uri = "/contact/create";
                return request.CreateRequest<Contact>( uri, Parameters.Contact( name, email ) );
            }
            return result;
        }


        /// <summary>
        /// This API call provides a possibility to get a contact list
        /// </summary>
        /// <param name="request">Api request params</param>
        /// <returns>List with user contacts</returns>
        public static IEnumerable<Contact> GetContacts ( this IQuatrixRequest request ) {
            request.Enable();                               //check if request not null
            string api_call = "/contact";                   //uri for get contacts list
            string json = request.CreateRequest( api_call );
            return JToken.Parse(json).ToObject<List<Contact>>();
        }
    }
}
