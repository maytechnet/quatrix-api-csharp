using System;
using NUnit.Framework;
using Maytech.Quatrix;


namespace Maytech.Tests.Authorization {


	[TestFixture]
    public class Logout {


        [Test]
        [Description( "Logout after login. Ok" )]
        public void Logout_AfterLogin_Ok() {
            QSession aut = Credential.Get();
            aut.Login();
            aut.Logout();
            Assert.IsFalse( aut.IsLoggedIn );
        }


        [Test]
        [Description( "Logout after login and logout. Ok" )]
        public void Logout_AfterLoginAndLogout_Ok() {
            QSession aut = Credential.Get();
            aut.Login();
            aut.Logout();
            Assert.IsTrue( aut.Logout() );
        }


        [Test]
        [Description( "Logout without login. Ok" )]
        public void Logout_WithoutLogin_Ok() {
            QSession aut = Credential.Get();
            Assert.IsTrue( aut.Logout() );
        }


    }
}
