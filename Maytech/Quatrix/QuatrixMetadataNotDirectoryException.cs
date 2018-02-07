namespace Maytech.Quatrix {


    public class QuatrixMetadataNotDirectoryException : QuatrixException {


        public QuatrixMetadataNotDirectoryException() : base( Messages.Error.api_metadata_not_directory, METADATA_NOT_DIRECTORY ) {
            Debug.Logger.Error( SETTINGS.APP_LOG_NAME, this, "QuatrixMetadataException" );
        }
    }
}
