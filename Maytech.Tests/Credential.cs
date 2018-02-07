using Maytech.Quatrix;

namespace Maytech.Tests {


    internal class Credential {

        //public const string Host = "am.nav.maytech.net";
        public const string Host = "dev.amashko.qx.maytech.net";
        public const string SuSpendedHost = "suspend.amashko.qx.maytech.net";
        public const string Email = "dima@maytech.net";
        public const string Password = "9379992";
        public const string Pass64 = "qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq";

        private static QSession cred;


        public static QSession Get() {
			return new QSession( Host, Email, Password );
        }


        public static QSession Log() {
            if( cred != null ) {
                return cred;
            }
            cred = new QSession( Host, Email, Password );
            cred.Remember = RememberOptions.All;
            cred.Login();
            return cred;
        }
    }
}
