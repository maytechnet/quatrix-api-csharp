using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Maytech.Quatrix.Tools {
    
    public static class Functions {

        public static bool SaveData<T> ( T objGraph, string savePath ) {
            BinaryFormatter binFormat = new BinaryFormatter();
            bool result = false;
            if (objGraph != null) {
                using (Stream fStream = new FileStream( savePath, FileMode.Create, FileAccess.Write, FileShare.None )) {
                    binFormat.Serialize( fStream, objGraph );
                    result = true;
                }
            }
            else if (File.Exists( savePath )) {
                File.Delete( savePath );
            }
            return result;
        }


        public static T LoadData<T> ( string loadPath ) {
            T result = default( T );
            if (File.Exists( loadPath )) {
                Stream fStream = null;
                try {
                    fStream = File.OpenRead( loadPath );
                    if (fStream.Length > 0) {
                        BinaryFormatter binFormat = new BinaryFormatter();
                        object data = binFormat.Deserialize( fStream );
                        if (data is T) {
                            result = (T)data;
                        }
                    }
                }
                catch {

                }
                finally {
                    if (fStream != null) {
                        fStream.Close();
                    }
                }
            }
            return result;
        }


        public static string GetTimestamp ( ) {
            //Get Unix TimeStamp
            return GetTimestamp( DateTime.UtcNow );
        }


        public static string GetTimestamp ( DateTime utcTime ) {
            //Get Unix TimeStamp
            TimeSpan ts = utcTime.Subtract( new DateTime( 1970, 1, 1 ) );
            return ( (int)ts.TotalSeconds ).ToString();
        }


        public static DateTime UnixTimeStampToDateTime ( double unixTimeStamp ) {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime( 1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc );
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dtDateTime;
        }
    }
}
