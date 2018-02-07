namespace Maytech.Quatrix {


    public class QuatrixUserExistsException : QuatrixException {


        internal QuatrixUserExistsException( QuatrixExceptionArgs args ) : base( args ) { }


        internal QuatrixUserExistsException( QuatrixExceptionArgs args, System.Exception innerException ) : base( args, innerException ) { }
    }
}