namespace Maytech.Quatrix.Entity {


    public enum MetadataType {
        NONE = 0,
        FILE_LINK = MetadataAttributes.FILE_LINK_TYPE,
        FILE = MetadataAttributes.FILE_TYPE,
        DIRECTORY = MetadataAttributes.DIRECTORY_TYPE,
        SYSTEM_DIRECTORY = MetadataAttributes.DIRECTORY_TYPE,
        USER_SHARED_FOLDER = MetadataAttributes.SHARED_TYPE | MetadataAttributes.DIRECTORY_TYPE,
        SYSTEM_SHARED_FOLDER = MetadataAttributes.SHARED_TYPE,
        TRASH = MetadataAttributes.TRASH_TYPE
    }
}
