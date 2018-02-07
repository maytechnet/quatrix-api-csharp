using System;
using NUnit.Framework;
using Maytech.Quatrix;
using System.IO;

namespace Maytech.Tests.Author {


    [TestFixture]
    public class Authorization {


        [Test]
        [Description( "Authorization with incorect host application layer = http, not a https. ExpectedException: QuatrixInvalidHostException; Quatrix name is incorrect." )]
        [ExpectedException( typeof( Maytech.Quatrix.Tools.Net.QuatrixInvalidHostException ) )]
        public void QSession_InitIncorectHost_Ex() {
            new QSession( "http://google.com" );
        }


        [Test]
        [Description( "Authorization with empty host. ExpectedException: UriFormatException; Invalid URI: The hostname could not be parsed." )]
        [ExpectedException( typeof( UriFormatException ) )]
        public void QSession_InitEmptyHost_Ex() {
            new QSession( string.Empty );
        }


        [Test]
        [Description( "Authorization with correct host and empty email.ExpectedException: ArgumentException; The parameter 'address' cannot be an empty string." )]
        [ExpectedException( typeof( ArgumentException ) )]
        public void QSession_InitEmptyEmail_Ex() {
            new QSession( Credential.Host, string.Empty );
        }


        [Test]
        [Description( "Authorization with correct host and invalid email. ExpectedException: FormatException; The specified string is not in the form required for an e-mail address." )]
        [ExpectedException( typeof( FormatException ) )]
        public void QSession_InitInvalidEmail_Ex() {
            new QSession( Credential.Host, "qqqqqq" );
        }


        [Test]
        [Description( "Authorization with correct host and email, and incorect password. ExpectedException: ArgumentException; Password should contain more than 5 symbols." )]
        [ExpectedException( typeof( ArgumentException ) )]
        public void QSession_InitWithPasLengthLess6Symbol_Ex() {
            new QSession( Credential.Host, Credential.Email, "12345" );
        }


        [Test]
        [Description( "Authorization with correct host and email, and incorect password. ExpectedException: ArgumentException; Password shouldn't contain more than 64 symbols." )]
        [ExpectedException( typeof( ArgumentException ) )]
        public void QSession_InitWithPasLengthMore64Symbol_Ex() {
            new QSession( Credential.Host, Credential.Email, Credential.Pass64 + "q" );
        }


        [Test]
        [Description( "Authorization with correct host and email, and null password. ExpectedException: NullReferenceException; User password is empty." )]
        [ExpectedException( typeof( NullReferenceException ) )]
        public void QSession_InitEmptylPas_Ex() {
            new QSession( Credential.Host, Credential.Email, string.Empty );
        }


        [Test]
        [Description( "Autorization with corect host. Ok" )]
        public void QSession_InitCorectHost_Ok() {
            new QSession( Credential.Host );
        }


        [Test]
        [Description( "Autorization with corect host and email. Ok" )]
        public void QSession_InitCorectHostEmail_Ok() {
            new QSession( Credential.Host, Credential.Email );
        }


        [Test]
        [Description( "Autorization with corect host, email and password with 6 symbol. Ok" )]
        public void QSession_InitWithMinPasLength_Ok() {
            new QSession( Credential.Host, Credential.Email, "123456" );
        }


        [Test]
        [Description( "Autorization with corect host, email and password with 64 symbol. Ok" )]
        public void QSession_InitWithMaxPasLength_Ok() {
            new QSession( Credential.Host, Credential.Email, Credential.Pass64 );
        }
    }
}
