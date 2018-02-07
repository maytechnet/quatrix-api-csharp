using System;
using NUnit.Framework;
using Maytech.Quatrix;
using Maytech.Quatrix.Operations;
using System.Collections.Generic;

namespace Maytech.Tests.Operations {


	[TestFixture]
    public class UserAction {


        static Profile p;
        static QSession ts;
        static QuotaInfo qtt;


		[SetUp]
        public void ClassInit() {
            ts = Credential.Log();
            p = ts.Profile;
            qtt = UserActions.GetUserQuotaInfo( ts );
        }


        [Test]
        [Description( "GetUserShareMetadata with correct credentials. Ok" )]
        public void GetUserShareMetadata_CorrectCred_Ok() {
            UserShareMetadata u = UserActions.GetUserShareMetadata( p );
            Assert.IsFalse( string.IsNullOrWhiteSpace( u.id ) );
            Assert.IsTrue( u.share_types.restricted_share );
            Assert.IsTrue( u.share_types.tracked_share );
            Assert.IsFalse( u.share_types.public_share );
        }
        

        [Test]
        [Description( "GetUserQuotaInfo with correct credentials. Ok" )]
        public void GetUserQuotaInfo_CorrectCred_Ok() {
            QuotaInfo qt = UserActions.GetUserQuotaInfo( ts );
            Assert.IsNotNull( qt.acc_limit );
            Assert.IsNotNull( qt.user_limit );
            Assert.IsNotNull( qt.user_used );
            Assert.IsNotNull( qt.acc_used );
        }


        [Ignore( "" )]
        [Test]
        [Description( "IsEnoughtSpace with user limit when size of the file less of the user quota. Ok" )]
        public void IsEnoghtSpace_UserLimitDataSizeLessQuota_Ok() {
            QSession qss = new QSession( Credential.Host, "dima+1@maytech.net", Credential.Password );
            qss.Login();
            QuotaInfo qtt = UserActions.GetUserQuotaInfo( qss );
            bool b = UserActions.IsEnoughtSpace( qss, qtt.user_limit - qtt.user_used - 1 );
            Assert.IsTrue( b );
        }


        [Ignore("")]
        [Test]
        [Description( "IsEnoughtSpace with user limit when size of the file equal to the user quota. Ok" )]
        public void IsEnoghtSpace_UserLimitDataSizeMoreQuota_Ok() {
            QSession qss = new QSession( Credential.Host, "dima+1@maytech.net", Credential.Password );
            qss.Login();
            QuotaInfo qtt = UserActions.GetUserQuotaInfo( qss );
            bool b = UserActions.IsEnoughtSpace( qss, qtt.user_limit - qtt.user_used );
            Assert.IsFalse( b );
        }


        [Test]
        [Description( "IsEnoughtSpace without user limit when size of the file less of the user quota. Ok" )]
        public void IsEnoghtSpace_AccLimitDataSizeLessQuota_Ok() {
            bool b = UserActions.IsEnoughtSpace( ts, qtt.acc_limit - qtt.acc_used - 1 );
            Assert.IsTrue( b );
        }


        [Test]
        [Description( "IsEnoughtSpace without user limit when size of the file equal to the user quota. Ok" )]
        public void IsEnoghtSpace_AccLimitDataSizeMoreQuota_Ok() {
            bool b = UserActions.IsEnoughtSpace( ts, qtt.acc_limit - qtt.acc_used );
            Assert.IsFalse( b );
        }


        [Test]
        [Description( "ResetPassword with correct credentials. Ok" )]
        public void ResetPassword_CorrectParams_Ok() {
            bool b = UserActions.ResetPassword( Credential.Host, "dima@maytech.net" );
            Assert.IsTrue( b );
        }
    }
}