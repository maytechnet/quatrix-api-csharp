namespace Maytech.Quatrix {


    public class QuatrixBlacklistException : QuatrixException {

        private const string IP_KEY = "ip";
        

        public string ip { get; private set; }


        internal QuatrixBlacklistException( QuatrixExceptionArgs args ) : base( args ) {
            if( args.details == null ) {
                return;
            }
            ip = args.details.Value<string>( IP_KEY );
        }
    }
}