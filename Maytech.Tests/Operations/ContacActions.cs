using NUnit.Framework;
using Maytech.Quatrix;
using Maytech.Quatrix.Operations;
using System.Collections.Generic;
using System.Linq;


namespace Maytech.Tests.Operations {


	[TestFixture]
    public class ContactAction {

        
        static QSession ts;
        static List<string> listId = new List<string> { };


		//Create new QSession and login befor tests work.
		[SetUp]
        public void ClassInit( ) {
            ts = Credential.Log();
        }



		[TearDown]
        public void ClassClean() {
			//Delete created contacts after testing
			foreach ( string id in listId ) {
                Tools.DeleteContact( ts, id );
            }
        }


        [Test]
        [Description( "CreateContact with empty name and check is contact name was created. Ok" )]
        public void CreateContact_EmptyName_Ok() {
            Profile p = ts.Profile;
            string email = "qq@maytech.net";
            Contact c = ContactActions.CreateContact( p, email, "" );
            Assert.IsFalse( string.IsNullOrWhiteSpace( c.email ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( c.id ) );
            listId.Add( c.id );
        }


        [Test]
        [Description( "CreateContact with empty name and check is create right name. Ok" )]
        public void CreateContact_EmptyNameAreEqual_Ok() {
            Profile p = ts.Profile;
            string email = "q@maytech.net";
            Contact c = ContactActions.CreateContact( p, email, "" );
            Assert.AreEqual( c.name, "q" );
            Assert.AreEqual( c.email, email );
            listId.Add( c.id );
        }


        [Test]
        [Description( "CreateContact with name and check is create right name. Ok" )]
        public void CreateContact_AreEqualCredentialNameAndCreateName_Ok() {
            Profile p = ts.Profile;
            string email = "qqqq@maytech.net";
            string name = "4q";
            Contact c = ContactActions.CreateContact( p, email, name );
            Assert.AreEqual( c.name, "4q" );
            listId.Add( c.id );
        }


        [Test]
		[Description( "CreateContact with empty email. ExpectedException: ArgumentException; The parameter 'address' cannot be an empty string." )]
        //[ExpectedException( typeof( System.ArgumentException ) )]
        public void CreateContact_EmptyEmail_Ex() {
			Assert.Throws<System.ArgumentException>( () => {
				Profile p = ts.Profile;
				ContactActions.CreateContact( p, string.Empty, "" );
			} );
		}


        [Test]
		[Description( "CreateContact with invalid email. ExpectedException: FormatExceptio; The specifie string is not in the form required for an e-mail address." )]
        public void CreateContact_IvalidEmail_Ex() {
			Assert.Throws<System.FormatException>( () => {
				Profile p = ts.Profile;
				Contact c = ContactActions.CreateContact( p, "qqqqqqqq", "" );
			} );
        }


        [Test]
        [Description( "CreateContact with email like profile email and check is a parameters are equal. Ok" )]
        public void CreateContact_WithEmailLikeProfileEmail_Ok() {
            Profile p = ts.Profile;
            Contact c = ContactActions.CreateContact( p, Credential.Email, "" );
            Assert.AreEqual( c.id, p.id );
            Assert.AreEqual( c.email, p.email );
            Assert.AreEqual( c.Request, p.Request );
            Assert.AreEqual( c.name, p.realname );
            Assert.AreEqual( c.has_key, p.has_key );
        }


        [Test]
        [Description( "GetContacts with correct credentials. Ok" )]
        public void GetContacts_CorrectParams_Ok() {
            IEnumerable<Contact> c = ContactActions.GetContacts( ts );
            foreach(Contact ct in c ) {
                Assert.IsFalse( string.IsNullOrWhiteSpace( ct.email ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( ct.id ) );
            }
        }
    }
}