﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace HRM.Models
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class AuthAdoContainer : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new AuthAdoContainer object using the connection string found in the 'AuthAdoContainer' section of the application configuration file.
        /// </summary>
        public AuthAdoContainer() : base("name=AuthAdoContainer", "AuthAdoContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AuthAdoContainer object.
        /// </summary>
        public AuthAdoContainer(string connectionString) : base(connectionString, "AuthAdoContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AuthAdoContainer object.
        /// </summary>
        public AuthAdoContainer(EntityConnection connection) : base(connection, "AuthAdoContainer")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
    }

    #endregion

}