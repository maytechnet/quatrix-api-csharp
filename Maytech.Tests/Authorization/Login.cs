using System;
using NUnit.Framework;
using Maytech.Quatrix;
using System.IO;

namespace Maytech.Tests.Authorization {


	[TestFixture]
    public class Login {


        [Test]
		[Description( "Login with correct host and empty email. ExpectedException: ArgumenException; The parameter 'address' cannot be an empty string." )]
        public void Login_EmptytEmail_Ex() {
			QSession aut = new QSession( Credential.Host );
			Assert.Throws<ArgumentException>( () => { 
					aut.Email = string.Empty;
					aut.Login();
				} );
        }


        [Test]
		[Description( "Login with correct host and invalid(not) email. ExpectedException: FormatExeption; The specified string is not in the from required for a e-mail address." )]
        public void Login_InvalidEmail_Ex() {
            QSession aut = new QSession( Credential.Host );
			Assert.Throws<FormatException>( () => { 
					aut.Email = "qwer";
					aut.Login();
				} );
        }


        [Test]
		[Description( "Login with correct host and email, and with empty Password. ExpectedException: NullReferenceException; User password is empty." )]
        public void Login_EmptyPassword_Ex() {
            QSession aut = new QSession( Credential.Host );
            aut.Email = Credential.Email;
			Assert.Throws<System.NullReferenceException>( () => { 
					aut.Password = string.Empty;
					aut.Login();
				} );
        }


		[Test]
		[Description( "Login with correct host and email, and without Password. ExpectedException: NullReferenceException; User password is empty." )]
		public void Login_WithoutPassword_Ex() {
			QSession aut = new QSession( Credential.Host );
			aut.Email = Credential.Email;
			Assert.Throws<NullReferenceException>( () => { aut.Login(); } );
		}


        [Test]
		[Description( "Login with correct host and email, and incorect password. ExpectedException: ArgumentException; Password should contain more than 5 symbols." )]
        public void Login_WithPasLengthLess6Symbol_Ex() {
            QSession aut = new QSession( Credential.Host );
            aut.Email = Credential.Email;
			Assert.Throws<ArgumentException>( () => { 
					aut.Password = "12345";
					aut.Login();
				} );

        }


        [Test]
		[Description( "Login with correct host and email, and incorect password. ExpectedException: ArgumentException; Password shouldn't contain more than 64 symbols." )]
        public void Login_WithPasLengthMore64Symbol_Ex() {
            QSession aut = new QSession( Credential.Host );
            aut.Email = Credential.Email;
			Assert.Throws<ArgumentException>( () => { 
				aut.Password = Credential.Pass64 + "q"; 
				aut.Login();
			} ); 
        }


        [Test]
        [Description( "Login with correct credentials. Ok." )]
        public void Login_CorectParams_OK() {
            QSession aut = Credential.Get();
            Assert.IsTrue( aut.Login() );
        }


        [Test]
        [Description( "Login if user already logged in. Ok." )]
        public void Login_IfAlreadyLoggedIn_OK() {
            QSession aut = Credential.Get();
            aut.Login();
            Assert.IsTrue( aut.Login() );
        }


        [Test]
		[Description( "Login after logout. ExpectedException: NullReferenceException; User password is empty." )]
        public void Login_AfterLogout_Ex() {
            QSession aut = Credential.Get();
            aut.Login();
            aut.Logout();
			Assert.Throws<NullReferenceException>( () => { aut.Login(); } );
        }
    }
}
