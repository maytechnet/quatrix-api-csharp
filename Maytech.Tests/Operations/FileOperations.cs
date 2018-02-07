using NUnit.Framework;
using System;
using Maytech.Quatrix;
using Maytech.Quatrix.Operations;
using System.Collections.Generic;
using System.Linq;


namespace Maytech.Tests.Operations {


	[TestFixture]
	public class FileOperation {


		static Metadata m;
		static QSession qs;
		static List<Metadata> folders = new List<Metadata>();


		[SetUp]
		public void ClassInit() {
			qs = Credential.Log();
			m = FileOperations.GetMetadata( qs, "" );
		}


		[TearDown]
		public void ClassClean() {
			foreach( Metadata fl in folders ) {
				fl.Delete();
			}
		}


		[Test]
		[Description("GetMetadata with empty id. Get File Explorer metadata.")]
		public void GetMetadata_EmptyID_Ok() {
			Metadata mt = FileOperations.GetMetadata( qs, "" );
			Assert.AreEqual( mt.name, "dima@maytech.net" );
			Assert.AreEqual( mt.parent, "0c593d79-f5e5-47de-af15-71d60c815860" );
			Assert.AreEqual( mt.id, "57687fe5-fdef-4940-9c04-382d52fe1006" );
		}


		[Test]
		[Description( "GetMetada with invalid id. ExpectedException: QuatrixWebException; The remote server returned an error: (404) Not Found." )]
		//[ExpectedException( typeof( QuatrixWebException ), "The remote server returned an error: (404) Not Found." )]
		public void GetMetadata_InvalidID_Ex() {
			Assert.Throws<QuatrixWebException>( () => {
					FileOperations.GetMetadata( qs, "123" );
				} ); 
		}



		[Test]
		[Description("GetMetadata with outbox id. Ok")]
		public void GetMetadata_OutboxID_Ok() {
			Metadata mt = FileOperations.GetMetadata( qs, "a9456cff-7861-402b-8589-844f7318c671" );
			Assert.AreEqual( mt.name, "Outbox" );
			Assert.AreEqual( mt.parent, "57687fe5-fdef-4940-9c04-382d52fe1006" );
			Assert.AreEqual( mt.id, "a9456cff-7861-402b-8589-844f7318c671" );
		}


		[Test]
		[Description( "MakeDir with empty new folder name. ExpectedException: NullReferenceException; Folder name param is empty!" )]
		//[ExpectedException( typeof( NullReferenceException ), "Folder name param is empty!" )]
		public void MakeDir_EmptyName_Ex() {
			Assert.Throws<NullReferenceException>( () => {
					m.MakeDir( string.Empty );
				} );
		}


		[Test]
		[Description( "MakeDir with correct parameters. Ok" )]
		public void MakeDir_CorrectParams_Ok() {
			Metadata mt = m.MakeDir( "q" );
			Assert.AreEqual( mt.name, "q" );
			Assert.IsFalse( string.IsNullOrWhiteSpace( mt.parent ) );
			Assert.IsFalse( string.IsNullOrWhiteSpace( mt.id ) );
			folders.Add( mt );
		}


		[Test]
		[Description("FindMetadata empty name. Return Null")]
		public void FindMetadata_EmptyName_Ok() {
			Metadata mtt = FileOperations.FindMetadata( m, string.Empty );
			Assert.IsNull( mtt );
		}


		[Test]
		[Description("FindMetadata correct parameters. Ok")]
		public void FindMetadata_CorrectParams_OK() {
			Metadata mtt = FileOperations.FindMetadata( m, "TestFolder" );
			Assert.AreEqual( mtt.id, "4845c9d1-21d3-48c8-ae3e-69977cccbb30" );
			Assert.AreEqual( mtt.name, "TestFolder" );
			Assert.AreEqual( mtt.parent, "57687fe5-fdef-4940-9c04-382d52fe1006" );
		}
	}
}

