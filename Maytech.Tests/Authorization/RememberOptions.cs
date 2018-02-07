using System;
using NUnit.Framework;
using Maytech.Quatrix;

namespace Maytech.Tests.Authorization {
    

	[TestFixture]
    public class RememberOptions {


        [Test]
        [Description( "Login RememberOptions(All) Logout. Ok" )]
        public void RememberOptions_All_OK() {
			QSession aut = Credential.Get();
			aut.Remember = Quatrix.RememberOptions.All;
            aut.Login();
            aut.Logout();
            Assert.IsTrue( !string.IsNullOrWhiteSpace( aut.Host ) && !string.IsNullOrWhiteSpace( aut.Email ) && !string.IsNullOrWhiteSpace( aut.Password ) );
        }


        [Test]
        [Description( "Login after logout with remeberoptions(HostLogin). Ok" )]
        public void RememberOptions_HostLogin_Ok() {
			QSession aut = Credential.Get();
			aut.Remember = Quatrix.RememberOptions.HostLogin;
            aut.Login();
            aut.Logout();
            Assert.IsTrue( !string.IsNullOrWhiteSpace( aut.Host ) && !string.IsNullOrWhiteSpace( aut.Email ) && string.IsNullOrWhiteSpace( aut.Password ) );
        }


        [Test]
        [Description( "Login after logout with remeberoptions(Host). Ok" )]
        public void RememberOptions_Host_Ok() {
			QSession aut = Credential.Get();
			aut.Remember = Quatrix.RememberOptions.Host;
            aut.Login();
            aut.Logout();
            Assert.IsTrue( !string.IsNullOrWhiteSpace( aut.Host ) && string.IsNullOrWhiteSpace( aut.Email ) && string.IsNullOrWhiteSpace( aut.Password ) );
        }


        [Test]
        [Description( "Login after logout with remeberoptions(None). Ok" )]
        public void RememberOptions_None_Ok() {
            QSession aut = Credential.Get();
            aut.Login();
            aut.Logout();
            Assert.IsTrue( !string.IsNullOrWhiteSpace( aut.Host ) && !string.IsNullOrWhiteSpace( aut.Email ) && string.IsNullOrWhiteSpace( aut.Password ) );
        }
    }
}
