using System;
using System.Runtime.Serialization;

namespace Maytech.Quatrix {


    public class QuatrixException : Exception {

        #region API EXCEPTION CODES

        public const int UNKNOW_ERROR = -1;
        public const int BAD_PARAMETER_VALUE = 10;
        public const int MISSING_PARAMETER = 11;
        public const int BAD_HOSTNAME = 12;
        public const int NOT_FOUND = 20;
        public const int NOT_SUPPORTED = 21;
        public const int OBJECT_EXISTS = 30;
        public const int USER_EXISTS = 31;
        public const int CONTACT_EXISTS = 32;
        public const int RECTRICTION_ERROR = 50;
        public const int EXPIRED_ERROR = 60;
        public const int NOT_ALLOWED = 70;
        public const int DEACTIVATED = 80;
        public const int INACTIVE = 81;
        public const int NO_DATA = 90;
        public const int QUOTA_EXCEEDED = 122;

        #endregion

        public const int NOT_ALLOWED_WITHOUT_2FA_ERROR = 71;
        public const int NOT_ALLOWED_WITHOUT_PIN_ERROR = 73;
        public const int NOT_ALLOWED_USER_IN_BLACKLIST = 74;
        public const int WRONG_AUTH_CODE_ERROR = 83;
        public const int WRONG_USER_PIN_ERROR = 84;

        protected internal const int METADATA_NOT_DIRECTORY = 91;
        protected internal const int PARENT_METADATA_NOT_SET = 95;
        protected internal const int NOT_ALLOWED_ZERO_SIZE = 159;

        public int Code {
            get {
                return HResult;
            }
        }

        public QuatrixException( string message, int code ) : base( message ) {
            HResult = code;
        }

        internal QuatrixException( QuatrixExceptionArgs args ) : this( args.msg, args.code ) { }

        internal QuatrixException( QuatrixExceptionArgs args, Exception innerException ) : base( args.msg, innerException ) {
            HResult = args.code;
        }

        protected QuatrixException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        internal static QuatrixException Create( QuatrixExceptionArgs args, Exception innerException ) {
            switch( args.code ) {
                case BAD_HOSTNAME:
                return new QuatrixInvalidHostException( args, innerException );
                case NOT_FOUND:
                return new QuatrixObjectNotFoundException(args, innerException);
                case BAD_PARAMETER_VALUE:
                return new QuatrixParameterException( args, innerException );
                case OBJECT_EXISTS:
                return new QuatrixObjectExistsException( args, innerException );
                case NOT_ALLOWED_USER_IN_BLACKLIST:
                return new QuatrixBlacklistException( args );
                case NOT_ALLOWED:
                return new QuatrixNotAllowedException( args, innerException );
                case CONTACT_EXISTS:
                return new QuatrixContactExistsException( args, innerException );
                case USER_EXISTS:
                return new QuatrixUserExistsException( args, innerException );
                case RECTRICTION_ERROR:
                return new QuatrixPermissionDeniedException( args, innerException );
                case QUOTA_EXCEEDED:
                return new QuatrixQuotaExceededException(args, innerException);
                case NO_DATA:
                return new QuatrixObjectNotFoundException( args, innerException );
                default:
                return new QuatrixException( args, innerException );
            }
        }
    }
}
