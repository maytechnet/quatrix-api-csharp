using System;
using NUnit.Framework;
using Maytech.Quatrix;
using Maytech.Quatrix.Operations;
using Maytech.Quatrix.Entity;

namespace Maytech.Tests.Operations {
   

	[TestFixture]
    public class AccountAction {


        [Test]
		[Description( "GetAccountMetadata with empty host. ExpectedException: UriFormatException; Invalid URI: The hostname could not be parsed" )]
        public void GetAccountMetadata_EmptyHost_Ex() {
			Assert.Throws<UriFormatException>( () => AccountActions.GetAccountMetadata( string.Empty ));
        }


        [Test]
		[Description( "GetAccountMetadata with incorrect host. ExpectedException: QuatrixInvalidHostException; Quatrix name is incorrect." )]
        public void GetAccountMetadata_IncorrectHost_Ex() {
			Assert.Throws<Maytech.Quatrix.Tools.Net.QuatrixInvalidHostException>( () => AccountActions.GetAccountMetadata( "afddsf.net" ) );
        }


        [Test]
        [Description( "GetAccountMetadata with correct host. Ok" )]
        public void GetAccountMetadata_CorrectHost_OK() {
			AccountInfo mt = AccountActions.GetAccountMetadata( Credential.Host );
			Assert.AreEqual( mt.status, "A" );
			Assert.AreEqual( mt.language, "en_GB" );
			Assert.AreEqual( mt.title, "c# team container" );
        }


        [Test]
		[Description( "IsSuspended with empty host. ExpectedException: UriFormatException; Invalid URI: The hostname could not be parsed." )]
        public void IsSuspended_EmptyHost_Ex() {
			Assert.Throws<UriFormatException>( () => AccountActions.IsSuspended( string.Empty ) );
        }


        [Test]
		[Description( "IsSuspended with incorrect host. ExpectedException: QuatrixInvalidHostException; Quatrix name is incorrect." )]
        public void IsSuspended_IncorrectHost_Ex() {
			Assert.Throws<Maytech.Quatrix.Tools.Net.QuatrixInvalidHostException>( () => AccountActions.IsSuspended( "afddsf.net" ) );
        }


        [Test]
        [Description( "IsSuspended with not suspended host. Ok" )]
        public void IsSuspended_NotSuspendedHost_OK() {
            Assert.IsFalse( AccountActions.IsSuspended( Credential.Host ) );
        }


        [Test]
        [Description( "IsSuspended with suspended host. Ok" )]
        public void IsSuspended_SuspendedHost_Ok() {
            Assert.IsTrue( AccountActions.IsSuspended( Credential.SuSpendedHost ) );
        }


        [Test]
        [Description( "IsSuspended with authorization on not suspende host. Ok")]
        public void IsSuspended_AuthorNotSuspended_Ok() {
            QSession qs = new QSession( Credential.Host );
            Assert.IsFalse( qs.IsSuspended() );
        }


        [Test]
        [Description( "IsSuspended with authorization on suspende host. Ok" )]
        public void IsSuspended_AuthorSuspended_Ok() {
            QSession qs = new QSession( Credential.SuSpendedHost );
            Assert.IsTrue( qs.IsSuspended() );
        }
    }
}
