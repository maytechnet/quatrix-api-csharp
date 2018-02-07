using System;
using NUnit.Framework;
using Maytech.Quatrix;
using Maytech.Quatrix.Operations;

namespace Maytech.Tests.Operations {
    

	[TestFixture]
    public class ProfileActions {


        [Test]
        [Description( "GetProfile corect params. OK" )]
        public void GetProfile_CorrectParams_Ok() {
            Profile p = Credential.Log().GetProfile();
            Assert.IsNotNull( p );
            Assert.IsFalse( string.IsNullOrWhiteSpace( p.channel_id ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( p.email ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( p.rootId ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( p.realname ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( p.super_admin ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( p.id ) );
        }


        [Test]
        [Description( "GetProfile corect params, are equal login email and profile email. OK" )]
        public void GetProfile_CorrectParamsAreEqualEmail_Ok() {
            Profile p = Credential.Log().GetProfile();
            Assert.AreEqual( p.email, Credential.Email );
        }
    }
}
