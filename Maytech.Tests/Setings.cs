using System;
using NUnit.Framework;

namespace Maytech.Tests {

	[SetUpFixture]
	public class EmptyClass {

		[SetUp]		
		public void Set() {
			//System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Ssl3;
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback( delegate { return true; } );
		}
	}
}

