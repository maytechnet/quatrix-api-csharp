using Maytech.Quatrix.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maytech.Quatrix {


    [Serializable]
    public class QApi : QSession, IQuatrixSystemDirectories {

        [field: NonSerialized]
        private Metadata home;              //home dir
        [field: NonSerialized]
        private Metadata inbox;             //indox dir
        [field: NonSerialized]
        private Metadata outbox;            //outbox dir
        [field: NonSerialized]
        private Metadata trash;             //trash
        [field: NonSerialized]
        private Metadata sharedProjects;    //share projects


        /// <summary>
        /// Gets Quatrix user contact list 
        /// </summary>
        public IEnumerable<Contact> Contats {
            get {
                return this.GetContacts();
            }
        }


        public Metadata Home {
            get {
                if( home == null ) {
                    home = this.GetMetadata( Profile.home_id, true );
                }
                return home;
            }
        }


        public Metadata Inbox {
            get {
                if( inbox == null ) {
                    inbox = Home.content.FirstOrDefault( c => 
                    c.metadata != null && string.Compare( c.metadata.subtype, Metadata.INBOX_SUBTYPE, true ) == 0 );
                }
                return inbox;
            }
        }


        public Metadata Outbox {
            get {
                if( outbox == null ) {
                    outbox = Home.content.FirstOrDefault( c => 
                    c.metadata != null && string.Compare( c.metadata.subtype, Metadata.OUTBOX_SUBTYPE, true ) == 0 );
                }
                return outbox;
            }
        }


        public Metadata Trash {
            get {
                if( trash == null ) {
                    trash = Home.content.FirstOrDefault( c => c.Type == Entity.MetadataType.TRASH );
                }
                return trash;
            }
        }


        public Metadata SharedProjects {
            get {
                if( sharedProjects == null ) {
                    sharedProjects = Home.content.FirstOrDefault( c => 
                    c.metadata != null && string.Compare( c.metadata.subtype, Metadata.SHARED_PROJECTS_SUBTYPE, true ) == 0 );
                }
                return sharedProjects;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="QApi"/> class, with the specified host name.
        /// </summary>
        /// <param name="host"> Quatrix name where account is hosted</param>
        public QApi( string host )
            : base( host ) {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="QApi"/> class, with the specified host name and email.
        /// </summary>
        ///  <remarks>All parameters are validating  </remarks>
        /// <param name="host">Quatrix name where account is hosted</param>
        /// <param name="email">Email, used to log in Quatrix account</param>
        public QApi( string host, string email )
            : base( host, email ) {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="QApi"/> class, with the specified host name, email and password.
        /// </summary>
        /// <param name="host">Quatrix name where account is hosted</param>
        /// <param name="email">Email, used to log in Quatrix account</param>
        /// <param name="password">Users password</param>
        public QApi( string host, string email, string password )
            : base( host, email, password ) {
        }
    }
}
