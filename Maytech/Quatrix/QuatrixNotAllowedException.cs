namespace Maytech.Quatrix {


    public class QuatrixNotAllowedException : QuatrixException {


        /// <summary>
        /// Default ctor using for Suspended account
        /// </summary>
        public QuatrixNotAllowedException() : this( Messages.Error.api_account_suspended, NOT_ALLOWED ) { }


        public QuatrixNotAllowedException( string message, int code ) : base( message, code ) { }


        internal QuatrixNotAllowedException( QuatrixExceptionArgs args ) : base( args ) { }


        internal QuatrixNotAllowedException( QuatrixExceptionArgs args, System.Exception innerException ) : base( args, innerException ) { }
    }
}