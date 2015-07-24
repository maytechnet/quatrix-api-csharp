using System.Security.Cryptography;
using System.Text;

namespace Maytech.Quatrix.Tools.Security {

    public static class SecurityTools {

        public static byte[] GetPBKDF2( string password ) {
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes( password, 0 );
            rfc2898.IterationCount = 4096;
            return rfc2898.GetBytes( 32 );
        }


        public static string GetHexPassword(byte[] key) {
            return ByteToString( key );
        }


        public static string HMAC_SHA1( string signature, string key ) {
            byte[] keyByte = Encoding.UTF8.GetBytes( key );
            byte[] messageBytes = Encoding.UTF8.GetBytes( signature );
            byte[] hashmessage = null;

            using (HMACSHA1 hmacsha1 = new HMACSHA1( keyByte )) {
                hashmessage = hmacsha1.ComputeHash( messageBytes );
            }

            return ByteToString( hashmessage );
        }


        public static string ByteToString( byte[] buff ) {
            string sbinary = "";
            foreach (byte b in buff) {
                //Create string hex key on lowercase
                sbinary += b.ToString( "x2" );
            }
            return sbinary;
        }
    }
}
