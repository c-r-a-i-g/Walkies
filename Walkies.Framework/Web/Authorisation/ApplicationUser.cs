using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Walkies.Framework.Web.Authorisation.Models;
using Walkies.Framework.Cookies;
using Walkies.Framework.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Walkies.Framework.Web.Authorisation
{

    public class ApplicationUser : IdentityUser
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public ApplicationUser()
        {
            this.LockoutEnabled = true;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<ApplicationUser> manager, string authenticationType )
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync( this, authenticationType );
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Saves the user account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SaveState Save( ApplicationUserManager manager )
        {

            try
            {

                var applicationUser = manager.GetUser( this.UserName );
                manager.Update( applicationUser );

                var result = SaveState.Success( applicationUser.Id );
                return result;

            }

            catch( Exception )
            {
                return SaveState.Error;
            }

        }

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

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */


    }

}