using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Maytech.Quatrix.Operations {


    /// <summary>
    /// Provides a possibility to manage contacts.
    /// </summary>
    public static class ContactActions {


        /// <summary>
        /// Check for existence and add new contact with default name to contact list.
        /// Default name - first part of email. Method checks both user and contact list.
        /// </summary>
        /// <param name="profile">Users profile, where this operation will be performed.</param>
        /// <param name="email">Email of the contact (required)</param>
        /// <param name="name">Name of the contact (optional)</param>
        /// <exception cref="QuatrixWebException"></exception>
        /// <returns>New or existing contact if email is valid.Otherwise - null</returns>
        public static Contact CreateContact( this Profile profile, string email, string name = null ) {
            System.Net.Mail.MailAddress madr = Validation.Email( email );         //validate email
            //check if user try to create contact with own email
            if( email == profile.email ) {
                //return user data as contact (user can send share to himself)
                return new Contact() {
                    id = profile.id,
                    email = profile.email,
                    Request = profile.Request,
                    name = profile.name,
                    has_key = profile.has_key
                };
            }
            //Check if name parameter not set
            if( string.IsNullOrEmpty( name ) ) {
                //Name will created from email address
                name = madr.User;
            }
            name = name.Trim();
            //Create contact object
            object c = new {
                email = email,
                name = name
            };
            //return token
            return profile.Request.Post<Contact>( QRequest.CONTACT_CREATE, c );
        }


        /// <summary>
        /// Gets Quatrix user contact list 
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>List with user contacts</returns>
        public static IEnumerable<Contact> GetContacts( this IQuatrixRequest request ) {
            string json = request.Get( QRequest.CONTACT_LIST );
            return JToken.Parse( json ).ToObject<List<Contact>>();
        }
    }
}