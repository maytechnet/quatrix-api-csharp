namespace Maytech.Quatrix {


    public class QuatrixParameterException : QuatrixException {


        internal QuatrixParameterException( QuatrixExceptionArgs args ) : base( args ) { }


        internal QuatrixParameterException( QuatrixExceptionArgs args, System.Exception innerException ) : base( args, innerException ) { }
    }
}
