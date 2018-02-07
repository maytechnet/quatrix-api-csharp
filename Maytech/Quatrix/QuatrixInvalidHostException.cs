using Maytech.Quatrix.Messages;

namespace Maytech.Quatrix {


    public class QuatrixInvalidHostException : QuatrixException {


        public QuatrixInvalidHostException() : base( Error.api_host_invalid, BAD_HOSTNAME ) { }


        internal QuatrixInvalidHostException( QuatrixExceptionArgs args ) : base( args ) { }


        internal QuatrixInvalidHostException( QuatrixExceptionArgs args, System.Exception innerException ) : base( args, innerException ) { }
    }
}