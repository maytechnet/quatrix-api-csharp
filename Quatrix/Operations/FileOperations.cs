using Maytech.Quatrix.Messages;
using Maytech.Quatrix.Tools;
using Maytech.Quatrix.Tools.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maytech.Quatrix.Operations {

    /// <summary>
    /// This group of API calls describes operations which can be performed with files. 
    /// </summary>
    public static class FileOperations {

        #region Directory extension

        /// <summary>
        /// Return home metadata from user profile
        /// </summary>
        /// <param name="profile">user profile</param>
        /// <returns>home metadata</returns>
        public static Metadata GetRootMetadata ( this Profile profile ) {
            return ReturnMetadata( profile.Request, profile.rootId );
        }


        public static Metadata MakeDir ( this Metadata metadata, string folderName ) {
            return MakeDir( metadata, folderName, new Action( delegate {
                if (string.IsNullOrEmpty( folderName )) {
                    throw new QException( Error.directory_name_param_empty );
                }
            } ) );
        }


        private static Metadata MakeDir ( this Metadata metadata, string folderName, Action before ) {
            return metadata.Request.MakeApiCall<Metadata>( "/file/makedir", Parameters.MakeDir( metadata.id, folderName ),
                new Action( ( ) => {
                    if (!metadata.IsDirectory()) {
                        throw new QException( "Current metadata is not directory" );
                    }
                    before();
                } ) );
        }

        #endregion


        #region File extension



        #endregion


        /// <summary>
        /// Refresh metadata
        /// </summary>
        /// <param name="metadata">current metadata</param>
        /// <returns>refreshed metadata</returns>
        public static Metadata RefreshMetadata ( this Metadata metadata ) {
            if (metadata.IsFile()) {
                return metadata;
            }
            else {
                return ReturnMetadata( metadata.Request, metadata.id );
            }
        }


        /// <summary>
        /// Create request which search metadata by id and fill his child metada
        /// </summary>
        /// <param name="request">Request which include API user settings</param>
        /// <param name="id">Metadata id</param>
        /// <returns>Metadata with all params</returns>
        private static Metadata ReturnMetadata ( IQuatrixRequest request, string id ) {
            request.Enable();                                           //check if request not null
            string uri = string.Format( "/file/metadata/{0}", id );     //create uri for get metadata by id
            string json = request.CreateRequest( uri );
            Metadata metadata = request.ToEntity<Metadata>( json );
            JToken token = JToken.Parse( json )["content"];             //if request result OK, parse result
            //if metadata directory and have content
            if (token != null && metadata.IsDirectory()) {
                List<Metadata> children = token.
                    Select( c => c.ToObject<Metadata>() ).ToList();
                AddMetadataChild( metadata, children );                 //fill parent meatadata
            }
            return metadata;
        }


        /// <summary>
        /// If Add child metadata to root metadata
        /// </summary>
        /// <param name="parent">root metadata</param>
        /// <param name="child">list with childs</param>
        private static void AddMetadataChild ( Metadata parent, List<Metadata> child ) {
            if (child != null && parent.IsDirectory() && child.Count > 0) {
                var childs = new List<Metadata>();
                foreach (Metadata mtD in child) {
                    childs.Add( mtD );
                    mtD.Request = parent.Request;
                }
                parent.ChildMetadata = childs;
            }
        }


        /// <summary>
        /// Search file or folder by name, in current folder
        /// </summary>
        /// <param name="metadata">current folder</param>
        /// <param name="name">name of file or folder</param>
        /// <returns>finded metadata (if not finded return null)</returns>
        public static Metadata FindMetadata ( this Metadata metadata, string name ) {
            if (metadata.ChildMetadata != null && metadata.ChildMetadata.Count() > 0) {
                return metadata.ChildMetadata.FirstOrDefault( m => m.name == name );
            }
            else if (metadata.name == name) {
                return metadata;
            }
            else {
                return null;
            }
        }


        public static Metadata GetMetadata ( this Metadata metadata, string id ) {
            return ReturnMetadata( metadata.Request, id );
        }


        /// <summary>
        /// Check if cuurent metadata is file
        /// </summary>
        /// <param name="metadata">current metadata</param>
        /// <returns></returns>
        public static bool IsFile ( this Metadata metadata ) {
            return metadata.type == "F";
        }


        /// <summary>
        /// Check if cuurent metadata is directory
        /// </summary>
        /// <param name="metadata">current metadata</param>
        /// <returns></returns>
        public static bool IsDirectory ( this Metadata metadata ) {
            return metadata.type == "D";
        }
    }
}
