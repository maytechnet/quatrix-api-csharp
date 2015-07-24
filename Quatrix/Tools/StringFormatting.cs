using System;

namespace Maytech.Quatrix.Tools {
    
    internal static class StringFormatting {

        public static string FormatFileSize ( this long fSize ) {
            short count = (short)Math.Floor( Math.Log10( fSize ) ); // +1 ? [full]
            string[] typeName = { "B", "kB", "MB", "GB", "TB", "PB", "*(10^3) PB" };
            short multiples = (short)( count / 3 );
            decimal denominator = (long)Math.Pow( 10, count - count % 3 );
            return string.Format( "{0}\t{1}", Math.Round( fSize / denominator, 2 ), typeName[multiples] );
        }

    }
}
