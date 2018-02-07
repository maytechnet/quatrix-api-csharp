using Maytech.Quatrix.Entity;
using Maytech.Quatrix.Messages;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Maytech.Quatrix.Operations {


    /// <summary>
    /// Describes operations which can be performed with files and directories. 
    /// </summary>
    public static class FileOperations {


        /// <summary>
        /// Gets the metadata from specified parent folder by id.
        /// </summary>
        /// <param name="request">User session</param>
        /// <param name="id">Searching metadata uuid.</param>
        /// <returns><see cref="Metadata "/> instance that contains information about finded metadata</returns>
        public static Metadata GetMetadata( this IQuatrixRequest request, string id, bool content = true ) {
            string route = string.Format( QRequest.FILE_METADATA, id, Convert.ToInt16( content ) ); //create uri for get metadata by id
            Metadata metadata = request.Get<Metadata>( route );
            foreach( Metadata item in metadata.content ) {
                item.Request = request;
                item.Parent = metadata;
            }
            return metadata;
        }


        /// <summary>
        /// Creates new subdirectory from specified metadata on cloud and with specified name
        /// </summary>
        /// <param name="parent">Parent folder metadata.</param>
        /// <param name="folderName">New folders name.</param>
        /// <param name="resolve"></param>
        /// <returns><see cref="Metadata "/>instance that contains information about new created folder</returns>
        public static Metadata MakeDir( this Metadata parent, string folderName, bool resolve = true ) {
            if( string.IsNullOrEmpty( folderName ) ) {
                throw new NullReferenceException( Error.directory_name_param_empty );
            }
            if( parent.Type == MetadataType.FILE ) {
                throw new QuatrixMetadataNotDirectoryException();
            }
            IQuatrixRequest request = parent.Request;
            //Create upload object
            //object will ref chunked upload
            object d = new {
                target = parent.id,
                name = folderName,
                resolve = resolve
            };
            var result = request.Post<Metadata>( QRequest.FILE_MKDIR, d );
            result.Parent = parent;
            return result;
        }


        public static bool Delete( this IQEntity metadata ) {
            object oids = new {
                //select metadata id to delete
                ids = new string[] { metadata.id }
            };
            QIds result = metadata.Request.Post<QIds>( QRequest.FILE_DELETE, oids );
            return result.Ids.Any( i => string.Compare( metadata.id, i ) == 0 );
        }


        /// <summary>
        /// Move to Trash metadata in Quatrix
        /// </summary>
        /// <param name="metadatas">metadatas to delete</param>
        /// <returns>job id</returns>
        public static string Delete( this IEnumerable<IQEntity> metadatas ) {
            if( metadatas == null ) {
                throw new NullReferenceException() {
                    Source = "metadatas"
                };
            }
            IQEntity entity = metadatas.FirstOrDefault( m => m.Request != null );
            if( entity == null ) {
                throw new ArgumentException( "Can't find entity with API request property" );
            }
            object oids = new {
                //select metadata id to delete
                ids = metadatas.Select( m => m.id )
            };
            var job = entity.Request.Post<Job>( QRequest.FILE_DELETE, oids );
            return job.job_id;
        }


        /// <summary>
        /// Rename metadata
        /// </summary>
        /// <param name="metadata">target</param>
        /// <param name="name">new name</param>
        /// <returns>updated metadata</returns>
        public static Metadata Rename( this IQEntity metadata, string name, bool resolve = true ) {
            string api_call = string.Concat( QRequest.FILE_RENAME, metadata.id );
            object nm = new {
                name = name,
                resolve = resolve
            };
            return metadata.Request.Post<Metadata>( api_call, nm );
        }


        public static bool Move( this IQEntity target, IQEntity destination, bool resolve = true ) {
            object o = new {
                ids = new string[] { target.id },
                target = destination.id,
                resolve = resolve
            };
            QIds result = target.Request.Post<QIds>( QRequest.FILE_MOVE, o );
            return result.Ids.Any( i => string.Compare( target.id, i ) == 0 );
        }


        /// <summary>
        /// Move childs metadata to new parent folder
        /// </summary>
        /// <param name="parent">new parent</param>
        /// <param name="data">content to move in parent</param>
        /// <returns>job id</returns>
        public static string MoveIn( this IQEntity parent, IEnumerable<IQEntity> data, bool resolve = true ) {
            if( parent == null ) {
                throw new NullReferenceException() {
                    Source = "parent"
                };
            }
            object o = new {
                ids = data.Select( c => c.id ),
                target = parent.id,
                resolve = resolve
            };
            var job = parent.Request.Post<Job>( QRequest.FILE_MOVE, o );
            return job.job_id;
        }


        /// <summary>
        /// Get changes during some time interval in cloud directory
        /// </summary>
        /// <param name="metadata">cloud directory</param>
        /// <param name="from">start time point</param>
        /// <param name="to">end time point (optional). DEFAULT: current time</param>
        /// <returns></returns>
        public static Diff Diff( this IQEntity metadata, double from, double to = 0.0 ) {
            string api_call = string.Format( "/file/diff/{0}?from={1}", metadata.id, from );
            if( to > 0 ) {
                api_call = string.Concat( api_call, "&to=", to );
            }
            Diff diff = null;
            try {
               diff = metadata.Request.Get<Diff>( api_call );
            }
            catch {
                Console.WriteLine("Cannot get diff");
                return null;
            }
            if( diff.data != null ) {
                foreach( var item in diff.data ) {
                    item.Request = metadata.Request;
                }
            }
            
            return diff;
        }


        /// <summary>
        /// Refresh Request object in content
        /// </summary>
        /// <param name="parent"></param>
        public static void RefreshContent( this Metadata parent ) {
            if( parent == null ) {
                throw new NullReferenceException {
                    Source = "parent"
                };
            }
            if( parent.content == null ) {
                return;
            }
            foreach( Metadata item in parent.content ) {
                item.Request = parent.Request;
            }
        }
    }
}