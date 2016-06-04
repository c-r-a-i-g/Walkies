using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;

using Walkies.Framework.Web.Authorisation;
using System.Web;
using Microsoft.Owin.Security.Cookies;
using Walkies.Core.Configuration;

namespace Walkies.Framework.Initialisation.AppStartup
{
    public class ConfigureAuth : IAppStartup
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Execute( IAppBuilder app )
        {

            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext( ApplicationDbContext.Create );
            app.CreatePerOwinContext<ApplicationUserManager>( ApplicationUserManager.Create );
            app.CreatePerOwinContext<ApplicationSignInManager>( ApplicationSignInManager.Create ); // for forms

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString( "/login" ),
                ExpireTimeSpan = TimeSpan.FromSeconds( ApplicationSettings.Current.SessionTimeout ),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = WalkiesSecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromSeconds( ApplicationSettings.Current.SessionTimeout ),
                        regenerateIdentity: ( manager, user ) => user.GenerateUserIdentityAsync( manager, DefaultAuthenticationTypes.ApplicationCookie ) )
                }
            } );

            app.UseExternalSignInCookie( DefaultAuthenticationTypes.ExternalCookie );
            app.UseTwoFactorSignInCookie( DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes( 5 ) );
            app.UseTwoFactorRememberBrowserCookie( DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie );
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

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
