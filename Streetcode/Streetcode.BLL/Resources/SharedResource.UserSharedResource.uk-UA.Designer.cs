﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Streetcode.BLL.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SharedResource_UserSharedResource_uk_UA {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SharedResource_UserSharedResource_uk_UA() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Streetcode.BLL.Resources.SharedResource.UserSharedResource.uk-UA", typeof(SharedResource_UserSharedResource_uk_UA).Assembly);
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
        ///   Looks up a localized string similar to Неправильний пароль.
        /// </summary>
        internal static string IncorrectPassword {
            get {
                return ResourceManager.GetString("IncorrectPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Помилка в User Manager.
        /// </summary>
        internal static string UserManagerError {
            get {
                return ResourceManager.GetString("UserManagerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Користувача не знайдено.
        /// </summary>
        internal static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Користувач з такою поштою вже зареєстрований.
        /// </summary>
        internal static string UserWithSuchEmailExists {
            get {
                return ResourceManager.GetString("UserWithSuchEmailExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Користувача з такою поштою та іменем не знайдено.
        /// </summary>
        internal static string UserWithSuchEmailNotFound {
            get {
                return ResourceManager.GetString("UserWithSuchEmailNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Користувач з таким іменем вже зареєстрований.
        /// </summary>
        internal static string UserWithSuchUsernameExists {
            get {
                return ResourceManager.GetString("UserWithSuchUsernameExists", resourceCulture);
            }
        }
    }
}
