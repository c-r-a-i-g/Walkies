using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Walkies.Framework.Attributes;
using Walkies.Framework.BaseClasses;
using Walkies.Framework.Web.Session;

namespace Walkies.Framework.Web.Authorisation.Models
{
    public class LoginModel : PageModelBase
    {

        // --------------------------------------------------------------------------------------------------------

        #region Class Members

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Constructor and Intialisation

        /// <summary>
        /// Creates an empty instance of the newEntity
        /// </summary>
        public LoginModel()
        {
            this.Initialise();
        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Public Methods

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Private Methods

        /// <summary>
        /// Initialises the login model
        /// </summary>
        private void Initialise()
        {

            this.EnableAnimation = true;

            if( HttpContext.Current.Request.IsAuthenticated )
            {

                var user = UserSession.Current.GetUser();

                this.ApplicationName = "Walkies";
                this.UserName = user.UserName;
                this.HasAcceptedTerms = user.DateAgreedTerms.HasValue; // TODO: AND user.DateAgreedTerms >= date of current terms
                this.MustAcceptTerms = ( this.HasAcceptedTerms == false );
                this.IsFirstLogin = ( user.DateAgreedTerms.HasValue == false );

            }

            else
            {
                this.MustAcceptTerms = false;
            }

        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Static Methods

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Properties

        [Required]
        [Display( Name = "Username" )]
        public string UserName { get; set; }

        [Required]
        [DataType( DataType.Password )]
        [Display( Name = "Password" )]
        public string Password { get; set; }

        [Display( Name = "Remember me?" )]
        public bool RememberMe { get; set; }

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

        [RequireTrue( ErrorMessage = "You must accept the terms and conditions" )]
        [Display( Name = "I Accept the Terms & Conditions" )]
        public bool HasAcceptedTerms { get; set; }

        [Persist]
        public bool MustAcceptTerms { get; set; }

        [Persist]
        public bool IsFirstLogin { get; set; }

        public string ApplicationName { get; set; }

        public bool EnableAnimation { get; set; }


        //[Display( Name = "Your Name" )]
        //[Required( ErrorMessage = "Please enter in your name", AllowEmptyStrings = false )]
        //public string RequestName { get; set; }

        //[Display( Name = "Your Email Address" )]
        //[Required( ErrorMessage = "Please enter in your email address", AllowEmptyStrings = false )]
        //public string RequestEmailAddress { get; set; }

        //[Display( Name = "Your Company Name" )]
        //public string RequestCompany { get; set; }

        //[Display( Name = "Reason for Request" )]
        //public string RequestReason { get; set; }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Derived Properties

        #endregion

        // --------------------------------------------------------------------------------------------------------

    }
}
