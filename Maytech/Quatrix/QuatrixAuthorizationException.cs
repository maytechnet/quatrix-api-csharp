namespace Maytech.Quatrix {


    public class QuatrixAuthorizationException : QuatrixException {


        public QuatrixAuthorizationException() : base( Messages.Error.api_not_logined, NOT_ALLOWED ) { }


        public QuatrixAuthorizationException( string message ) : base( message, NOT_ALLOWED ) { }


        internal QuatrixAuthorizationException( QuatrixExceptionArgs args ) : base( args ) { }
    }
}
