namespace Maytech.Quatrix {


    public class QuatrixObjectExistsException : QuatrixException {


        internal QuatrixObjectExistsException( QuatrixExceptionArgs args ) : base( args ) { }


        internal QuatrixObjectExistsException( QuatrixExceptionArgs args, System.Exception innerException ) : base( args, innerException ) { }
    }
}
