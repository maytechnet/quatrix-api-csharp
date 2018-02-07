namespace Maytech.Quatrix {


    public class QuatrixPermissionDeniedException : QuatrixException {


        public QuatrixPermissionDeniedException() : base( Messages.Error.api_login_not_allowed, RECTRICTION_ERROR ) { }


        internal QuatrixPermissionDeniedException( QuatrixExceptionArgs args ) : base( args ) { }


        internal QuatrixPermissionDeniedException( QuatrixExceptionArgs args, System.Exception innerException ) : base( args, innerException ) { }
    }
}
