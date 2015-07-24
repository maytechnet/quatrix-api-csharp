using Newtonsoft.Json.Linq;

namespace Maytech.Quatrix.Tools {
    
    internal static class Parameters {


        public static byte[] Contact ( string name, string email ) {
            //Create contact object
            object c = new {
                email = email,
                name = name
            };
            //return token
            return c.JData();            
        }


        public static byte[] Upload ( string idMetadata, string fileName ) {
            //Create upload object
            //object will ref chunked upload
            object u = new {
                parent = idMetadata,
                name = fileName,
                upload_type = "chunked"
            };
            //return token
            return u.JData();            
        }


        public static byte[] MakeDir ( string idMetadata, string dirName ) {
            //Create upload object
            //object will ref chunked upload
            object d = new {
                target = idMetadata,
                name = dirName,
            };
            //return token
            return d.JData();            
        }


        public static byte[] ShareDownloadLink ( string id, string[] links ) {
            object s = new {
                id = id,
                links = links
            };
            return s.JData();            
        }


        public static byte[] ResetPass ( string email ) {
            object obj = new {
                email = email
            };
            return obj.JData();            
        }
            

        public static string GetJtokenString ( object obj ) {
            return JToken.FromObject( obj ).ToString();
        }


        private static byte[] JData ( this object obj ) {
            string jdata = GetJtokenString( obj );
            return System.Text.Encoding.UTF8.GetBytes( jdata );
        }
    }
}
