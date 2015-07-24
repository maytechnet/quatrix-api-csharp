using Maytech.Quatrix.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maytech.Quatrix {

    [Serializable]
    public class QApi : QAuthentication {

        private Metadata root;


        /// <summary>
        /// Return user metadata
        /// </summary>
        public Metadata RootMeatadata {
            get {
                if (root == null) {
                    root = Profile.GetRootMetadata();
                }
                return root;
            }
        }


        public IEnumerable<Contact> Contats {
            get {
                return this.GetContacts();
            }
        }


        public IEnumerable<User> Users {
            get {
                return this.GetUsers();
            }
        }


        public QApi ( ) {

        }


        public QApi ( string host, string email )
            : base( host, email ) {
        }


        public QApi ( string host, string email, string password )
            : base( host, email, password ) {
        }


        public static bool ResetPassword ( string host, string email ) {
            return UserActions.ResetPassword( host, email );
        }


        #region Metadata



        #endregion


        #region Share



        #endregion


        #region Contacts

        public bool ExistContact ( string email ) {
            return Contats.Any( c => c.email == email );
        }


        public Contact AddContact ( string email ) {
            return AddContact( email, string.Empty );
        }


        public Contact AddContact ( string email, string name ) {
            //var contacts = this.Contats;
            Contact contact = null;
            if (!string.IsNullOrEmpty( email )) {
                email = email.Trim().ToLower();
                contact = this.Contats.FirstOrDefault( c => c.email == email );     //Check if contacts already constains in our "contact list" 
                if (contact == null) {
                    var user = this.Users.FirstOrDefault( u => u.email == email );
                    if (user != null) {
                        contact = new Contact {
                            id = user.id,
                            name = user.realname,
                            //has_key      ???
                            Request = user.Request,
                            created = user.created,
                            status = user.status
                        };
                    }
                    else {
                        name = string.Empty;
                        try {
                            contact = this.Profile.CreateContact( email, name );
                        }
                        catch {
                            //EXCEPTION IN CREATE PARAMETER!
                        }
                    }
                }
            }
            return contact;
        }

        #endregion


        #region Upload


        #endregion
    }
}
