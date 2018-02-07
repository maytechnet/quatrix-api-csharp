using Maytech.Quatrix.Messages;
using Maytech.Quatrix.Operations;
using System;
using System.IO;
using System.Linq;

namespace Maytech.Quatrix.Transfer {


    /// <summary>
    /// Provides possibility to upload folder to Quatrix. This class can't be inherited
    /// </summary>
    public class FolderUpload<T> : IQuatrixTransfer where T : FileChunkUpload {


        private Metadata parent;
        private long length = -1;


        /// <summary>
        /// Occurs when uploading is started.
        /// </summary>
        public virtual event EventHandler Started;


        /// <summary>
        /// Occurs when uploading is finished.
        /// </summary>
        public virtual event EventHandler Finished;


        /// <summary>
        /// Gets or sets count of uploaded bytes.
        /// </summary>
        public virtual long Position { get; protected set; }


        /// <summary>
        /// Gets information about uploaded file or folder.
        /// </summary>
        public Metadata Result { get; private set; }


        /// <summary>
        /// Gets file size in bytes .
        /// </summary>
        public virtual long Length {
            get {
                if( length == -1 ) {
                    length = CalculateTotalDirSize( CurrentDirectory );
                }
                return length;
            }
        }


        public bool AllowZeroSize { get; set; }


        public bool Recursive { get; set; }


        protected virtual int ChunkSize { get; set; }


        protected internal string ParentId { get; set; }


        protected internal IQuatrixRequest Session { get; set; }


        /// <summary>
        /// Gets information about current directory.
        /// </summary>
        public DirectoryInfo CurrentDirectory { get; private set; }


        /// <summary>
        /// Gets the full path to file.
        /// </summary>
        public string FullName {
            get {
                return CurrentDirectory.FullName;
            }
        }


        /// <summary>
        /// Gets or sets the path on cloud where files should be uploaded.
        /// </summary>
        public Metadata Parent {
            get {
                return parent;
            }
            set {
                parent = value;
                if( parent != null ) {
                    this.ParentId = parent.id;
                    this.Session = parent.Request;
                }
            }
        }


        public bool ResolveName { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FolderUpload"/> class  with specified path to local directory.
        /// </summary>
        /// <param name="path">The path to local directory</param>
        /// <exception cref="DirectoryNotFoundException">
        /// If directory doesn't exist
        /// </exception>
        protected internal FolderUpload( string path, int fileChunkSize ) : this( new DirectoryInfo( path ), fileChunkSize ) { }


        protected internal FolderUpload( DirectoryInfo dirInfo, int fileChunkSize ) {
            //check if chunk size less then 1kB
            if( Validation.ChunkSize( fileChunkSize ) ) {
                ChunkSize = fileChunkSize;
            }
            CurrentDirectory = dirInfo;
            AllowZeroSize = true;
            Recursive = true;
            ResolveName = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="parent"></param>
        /// <returns>null for break upload</returns>
        protected virtual T OnFileUploadInitialize( string filePath, Metadata parent ) {
            return new FileChunkUpload( filePath, ChunkSize ) as T;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="parent"></param>
        /// <returns>null for break upload</returns>
        protected virtual FolderUpload<T> OnFolderUploadInitialize( DirectoryInfo folder, Metadata parent ) {
            FolderUpload<T> fu = new FolderUpload<T>( folder, ChunkSize );
            fu.AllowZeroSize = AllowZeroSize;
            fu.Recursive = Recursive;
            return fu;
        }



        /// <summary>
        /// Starts folder uploading and sets status to "Upload"
        /// </summary>
        public virtual void Start() {
            if( !AllowZeroSize && length == 0 ) {
                throw new QuatrixNotAllowedException( Error.operation_not_allowed_folder_zero_size, QuatrixException.NOT_ALLOWED_ZERO_SIZE );
            }
            //directory was not renamed?
            if( Started != null ) {
                Started( this, new EventArgs() );
            }
            OnFolderCreate();
            Upload( Result, CurrentDirectory );
            OnFinish();
        }


        /// <summary>
        /// Make dir which stored in <paramref name="Result"/> property
        /// </summary>
        protected virtual void OnFolderCreate() {
            if(string.IsNullOrEmpty(ParentId)) {
                throw new ArgumentNullException("Parent", Error.api_parent_metadata_null);
            }
            if(Parent == null) {
                Parent = new Metadata { id = ParentId, Request = Session };
            }
            Result = FileOperations.MakeDir(Parent, CurrentDirectory.Name, ResolveName);
            Result.type = "D";
        }


        /// <summary>
        /// Raise Finished event
        /// </summary>
        protected virtual void OnFinish() {
            if( this.Finished != null ) {
                this.Finished( this, new EventArgs() );
            }
        }


        protected virtual void Upload( Metadata parent, DirectoryInfo parentInfo ) {
            //========================== Upload files ===================================
            FileInfo[] files = parentInfo.GetFiles();           //get files from folder
            foreach( var file in files ) {
                T upload = OnFileUploadInitialize( file.FullName, parent );
                if( upload == null ) {
                    continue;
                }
                upload.Parent = parent;
                upload.Start();
            }
            //============================================================================
            if( !Recursive ) {
                return;
            }
            DirectoryInfo[] ds = parentInfo.GetDirectories();
            foreach( var d in ds ) {
                //Check for skip all empty dirs
                FolderUpload<T> upload = OnFolderUploadInitialize( d, parent );                
                if( upload == null ) {
                    continue;
                }
                upload.Parent = parent;
                if( upload.AllowZeroSize || upload.Length > 0 ) {
                    upload.Start();
                }
            }
        }


        protected long CalculateTotalDirSize( DirectoryInfo dir ) {
            long length = CalculateDirSize( dir );
            //dir Length
            foreach( var d in dir.GetDirectories() ) {
                length += CalculateTotalDirSize( d );
            }
            return length;                                  //our result Length
        }


        private long CalculateDirSize( DirectoryInfo dir ) {
            return dir.GetFiles().Sum( s => s.Length );   //file Length
        }
    }
}