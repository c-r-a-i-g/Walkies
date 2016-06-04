using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Walkies;

using Walkies.Database;
using Walkies.Database.Entities;
using Walkies.Framework.BaseClasses;
using Walkies.Framework.Managers;
using Walkies.Framework.Models.Admin;
using Walkies.Framework.Web;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Session;

namespace Walkies.Framework.Models.Admin
{
    public class UserModel : DataPageModelBase<UserManager, UserModel, User, Guid>
    {

        // --------------------------------------------------------------------------------------------------------

        #region Class Members

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Constructor and Intialisation

        /// <summary>
        /// Creates an empty instance of the model
        /// </summary>
        public UserModel()
        {

        }

        /// <summary>
        /// Creates a mapped instance of the model
        /// </summary>
        public UserModel( Guid userId ) : base( userId )
        {
            this.Intialise();
        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Public Methods

        public override void OnAfterBind( User entity )
        {
            base.OnAfterBind( entity );

        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Private Methods

        /// <summary>
        /// Initialises the model
        /// </summary>
        private void Intialise()
        {


        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Static Methods

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Properties

        [Key, Persist]
        public Guid? UserId { get; set; }

        [Required, DisplayName( "Role" )]
        public Guid RoleId { get; set; }

        [DisplayName( "Username" ), MaxLength( 150 )]
        public string UserName { get; set; }

        [Required, DisplayName( "First Name" ), MaxLength( 50 )]
        public string FirstName { get; set; }

        [Required, DisplayName( "Last Name" ), MaxLength( 50 )]
        public string LastName { get; set; }

        [Required, DisplayName( "Email" ), EmailAddress, MaxLength( 255 )]
        public string Email { get; set; }

        public Guid? ApplicationUserId { get; set; }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Derived Properties

        [NotMapped]
        public bool IsEditing
        {
            get { return this.UserId.HasValue && this.UserId != Guid.Empty; }
        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

    }
}