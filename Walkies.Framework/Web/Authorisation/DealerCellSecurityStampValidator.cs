using System;
using System.Data.Entity.SqlServer.Utilities;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Walkies.Framework.Web.Authorisation
{
    public static class WalkiesSecurityStampValidator
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
        ///     Can be used as the ValidateIdentity method for a CookieAuthenticationProvider which will check a user's security
        ///     stamp after validateInterval
        ///     Rejects the identity if the stamp changes, and otherwise will call regenerateIdentity to sign in a new
        ///     ClaimsIdentity
        /// </summary>
        /// <typeparam name="TManager"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="validateInterval"></param>
        /// <param name="regenerateIdentity"></param>
        /// <returns></returns>
        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser>(
            TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentity )
            where TManager : UserManager<TUser, string>
            where TUser : class, IUser<string>
        {
            return OnValidateIdentity( validateInterval, regenerateIdentity, id => id.GetUserId() );
        }

        /// <summary>
        ///     Can be used as the ValidateIdentity method for a CookieAuthenticationProvider which will check a user's security
        ///     stamp after validateInterval
        ///     Rejects the identity if the stamp changes, and otherwise will call regenerateIdentity to sign in a new
        ///     ClaimsIdentity
        /// </summary>
        /// <typeparam name="TManager"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="validateInterval"></param>
        /// <param name="regenerateIdentityCallback"></param>
        /// <param name="getUserIdCallback"></param>
        /// <returns></returns>
        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser, TKey>(
            TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentityCallback,
            Func<ClaimsIdentity, TKey> getUserIdCallback )
            where TManager : UserManager<TUser, TKey>
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
        {
            return async context =>
            {
                var currentUtc = DateTimeOffset.UtcNow;
                if ( context.Options != null && context.Options.SystemClock != null )
                {
                    currentUtc = context.Options.SystemClock.UtcNow;
                }
                var issuedUtc = context.Properties.IssuedUtc;

                // Only validate if enough time has elapsed
                var validate = ( issuedUtc == null );
                if ( issuedUtc != null )
                {
                    var timeElapsed = currentUtc.Subtract( issuedUtc.Value );
                    validate = timeElapsed > validateInterval;
                }
                if ( validate )
                {
                    var manager = context.OwinContext.GetUserManager<TManager>();
                    var userId = getUserIdCallback( context.Identity );
                    if ( manager != null && userId != null )
                    {
                        var user = await manager.FindByIdAsync( userId ).WithCurrentCulture();
                        var reject = true;
                        // Refresh the identity if the stamp matches, otherwise reject
                        if ( user != null && manager.SupportsUserSecurityStamp )
                        {
                            var securityStamp =
                                context.Identity.FindFirstValue( Constants.DefaultSecurityStampClaimType );
                            if ( securityStamp == await manager.GetSecurityStampAsync( userId ).WithCurrentCulture() )
                            {
                                reject = false;
                                // Regenerate fresh claims if possible and resign in
                                if ( regenerateIdentityCallback != null )
                                {
                                    var identity = await regenerateIdentityCallback.Invoke( manager, user );
                                    if ( identity != null )
                                    {
                                        // here you need to do magic with the context
                                        // should contain previous cookie value if it was set to be persistent. 
                                        var isPersistent = context.Properties.IsPersistent;

                                        // and now you should set this flag on the new cookie
                                        context.OwinContext.Authentication.SignIn( new AuthenticationProperties { IsPersistent = isPersistent }, identity );
                                    }
                                }
                            }
                        }
                        if ( reject )
                        {
                            context.RejectIdentity();
                            context.OwinContext.Authentication.SignOut( context.Options.AuthenticationType );
                        }
                    }
                }
            };
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
