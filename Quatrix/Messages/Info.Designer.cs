﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Maytech.Quatrix.Messages {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Info {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Info() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Maytech.Quatrix.Messages.Info", typeof(Info).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Created (Code: 201).
        /// </summary>
        internal static string request_created {
            get {
                return ResourceManager.GetString("request_created", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OK (Code: 200).
        /// </summary>
        internal static string request_ok {
            get {
                return ResourceManager.GetString("request_ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to only logged-in users who were in the list of recipients can download files/folders.
        /// </summary>
        internal static string share_confidential {
            get {
                return ResourceManager.GetString("share_confidential", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to either logged-in or non-logged-in users can download files/folders.
        /// </summary>
        internal static string share_public {
            get {
                return ResourceManager.GetString("share_public", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Qutrix share policy.
        /// </summary>
        internal static string share_quatrix {
            get {
                return ResourceManager.GetString("share_quatrix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to any logged-in users can download files/folders.
        /// </summary>
        internal static string share_tracked {
            get {
                return ResourceManager.GetString("share_tracked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to canceled.
        /// </summary>
        internal static string status_canceled {
            get {
                return ResourceManager.GetString("status_canceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to disconnected.
        /// </summary>
        internal static string status_disconnected {
            get {
                return ResourceManager.GetString("status_disconnected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to status.
        /// </summary>
        internal static string status_name {
            get {
                return ResourceManager.GetString("status_name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to suspended.
        /// </summary>
        internal static string status_suspend {
            get {
                return ResourceManager.GetString("status_suspend", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to upload.
        /// </summary>
        internal static string status_upload {
            get {
                return ResourceManager.GetString("status_upload", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to uploaded.
        /// </summary>
        internal static string status_uploaded {
            get {
                return ResourceManager.GetString("status_uploaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to waiting.
        /// </summary>
        internal static string status_waiting {
            get {
                return ResourceManager.GetString("status_waiting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quatrix uploader.
        /// </summary>
        internal static string upload_quatrix {
            get {
                return ResourceManager.GetString("upload_quatrix", resourceCulture);
            }
        }
    }
}