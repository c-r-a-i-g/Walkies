﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Framework.BaseClasses;

namespace Walkies.Framework.Web.Authorisation.Models
{
    public class ChangePasswordPageModel : PageModelBase
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public string ReturnUrl { get; set; }

        [Required, Persist]
        public Guid UserId { get; set; }

        [Required]
        [DataType( DataType.Password )]
        [Display( Name = "Current password" ), MaxLength( 50 )]
        public string OldPassword { get; set; }

        [Required]
        [StringLength( 100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6 )]
        [DataType( DataType.Password )]
        [Display( Name = "New password" ), MaxLength( 50 )]
        public string NewPassword { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "Confirm new password" ), MaxLength( 50 )]
        [Compare( "NewPassword", ErrorMessage = "The new password and confirmation password do not match." )]
        public string ConfirmPassword { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
