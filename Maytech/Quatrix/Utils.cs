using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace Maytech.Quatrix {


    /// <summary>
    /// Contains help methods to make correct work with API lib
    /// </summary>
    public static class Utils {


        public static double GetTimestamp( DateTime utcTime ) {
            return utcTime.Subtract( new DateTime( 1970, 1, 1 ) ).TotalSeconds;
        }


        /// <summary>
        /// Gets UNIX timestamp.
        /// </summary>
        /// <returns>Unix timestamp</returns>
        public static string GetTimestampString() {
            //Get Unix TimeStamp
            return GetTimestampString( DateTime.UtcNow );
        }


        /// <summary>
        /// Gets UNIX timestamp from specified time.
        /// </summary>
        /// <param name="utcTime">Time in UTC format to converting.</param>
        /// <returns>Unix timestamp</returns>
        public static string GetTimestampString( DateTime utcTime ) {
            //Get Unix TimeStamp
            TimeSpan ts = utcTime.Subtract( new DateTime( 1970, 1, 1 ) );
            return ((int)ts.TotalSeconds).ToString();
        }
    }
}