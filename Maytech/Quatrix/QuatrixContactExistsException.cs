namespace Maytech.Quatrix {


    public class QuatrixContactExistsException : QuatrixException {


        internal QuatrixContactExistsException( QuatrixExceptionArgs args ) : base( args ) { }


        internal QuatrixContactExistsException( QuatrixExceptionArgs args, System.Exception innerException ) : base( args, innerException ) { }
    }
}
