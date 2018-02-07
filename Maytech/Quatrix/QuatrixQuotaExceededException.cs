using System;

namespace Maytech.Quatrix {


    public class QuatrixQuotaExceededException : QuatrixException {


        internal QuatrixQuotaExceededException( QuatrixExceptionArgs args, Exception innerException ) : base( args, innerException) { }
    }
}
