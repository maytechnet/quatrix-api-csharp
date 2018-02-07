namespace Maytech.Quatrix {


    public class QuatrixShareException : QuatrixException {

        protected internal const int SHARE_WITHOUT_FILES = 100;
        protected internal const int SHARE_WITHOUT_RECEIVERS = 101;
        protected internal const int INVALID_EXPIRE_DATE = 102;
        protected internal const int INVALID_RELEASE_DATE = 103;


        public QuatrixShareException( string msg, int code ) : base( msg, code ) {
            Debug.Logger.Error( SETTINGS.APP_LOG_NAME, this, "QuatrixShareException" );
        }
    }
}
