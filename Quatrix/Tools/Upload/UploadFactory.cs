using System.IO;

namespace Maytech.Quatrix.Tools.Upload {

    public static class UploadFactory {

        public static IQuatrixUpload GetUploader ( string path ) {
            IQuatrixUpload upl;
            if (IsDirectory( path )) {
                DirectoryUpload dirUpl = new DirectoryUpload( path );
                dirUpl.Recursive = true;
                upl = dirUpl;
            }
            else {
                upl = new ChunkedFileUpload( path );
            }
            return upl;
        }


        internal static bool IsDirectory ( string path ) {
            FileAttributes attr = File.GetAttributes( path );
            return ( attr & FileAttributes.Directory ) == FileAttributes.Directory;
        }

    }
}
