using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Walkies.Framework.Web.Authorisation.Models;
using Walkies.Framework.Web.Authorisation;
using Walkies.Framework.Web.Session;
using Walkies.Framework.BaseClasses;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Enumerations;
using Walkies.Database;
using Walkies.Framework.Managers;
using Walkies.Core.Enumerations;
using System.Diagnostics;
using System.Threading;

namespace Walkies.Framework.Web.Authorisation.Controllers
{

    public partial class AccountController : WebControllerBase
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
        /// Gets the login page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route( "login" )]
        public ActionResult Login( string returnUrl )
        {

            ViewBag.ReturnUrl = returnUrl;

            var model = new LoginModel();
            return View( model );

        }

        /// <summary>
        /// Performs a login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route( "login" )]
        public async Task<ActionResult> Login( LoginModel model, string returnUrl )
        {
            
            if( model.IsFirstLogin == false )
            {
                ModelState.Remove( "ConfirmPassword" );
                ModelState.Remove( "NewPassword" );
                ModelState.Remove( "OldPassword" );
                ModelState.Remove( "IsFirstLogin" );
            }

            if( model.MustAcceptTerms == false )
            {
                ModelState.Remove( "HasAcceptedTerms" );
            }

            if( model.IsFirstLogin || model.MustAcceptTerms )
            {
                ModelState.Remove( "Username" );
                ModelState.Remove( "Password" );
            }

            if( ModelState.IsValid == false )
            {
                goto LOGIN_ERROR;
            }

            // Determine if the user account is currently active
            var db = new WalkiesDB();
            var user = db.Users.FirstOrDefault( u => u.UserName == model.UserName && u.IsActive );

            if( user != null )
            {

                if( user.IsLocked )
                {
                    goto LOCKED_OUT;
                }

                if( model.IsFirstLogin || model.MustAcceptTerms )
                {

                    var manager = new UserManager();
                    var result = manager.SaveAcceptTerms( user.UserId, model );

                    if ( result.IsSuccessful )
                    {
                        Session.Remove( "TempPassword" );
                        UserSession.Refresh();
                        
                        return RedirectToLocal( "/" );
                    }

                    else
                    {
                        return ViewWithNotification( model, result.Data, NotificationType.Danger );
                    }

                }

                else
                {

                    var result = await SignInManager.PasswordSignInAsync( model.UserName, model.Password, model.RememberMe, shouldLockout: true );

                    switch( result )
                    {

                        case SignInStatus.Success:

                            user.DateLastLogin = DateTime.Now;
                            user.InvalidLoginAttempts = 0;
                            db.SaveChanges();

                            SignInManager.UserManager.ResetAccessFailedCount( user.UserId.ToString() );

                            UserSession.Login( model.UserName );
                            Session[ "OnLogIn" ] = true;
                            
                            if( user.DateAgreedTerms.HasValue == false ) // TODO: || user.DateAgreedTerms < DATE OF TERMS)
                            {
                                Session[ "TempPassword" ] = model.Password;
                                model.EnableAnimation = false;

                                return JsonContent( new
                                {
                                    hasForm = true,
                                    formHtml = base.RenderPartialToString( "AcceptTerms", new AcceptTermsAndChangePasswordPageModel() { MustAcceptTerms = true, IsFirstLogin = ( user.DateAgreedTerms.HasValue == false ) } )
                                } );
                            }
                            
                            return RedirectToLocal( returnUrl );

                        case SignInStatus.LockedOut:
                            user.InvalidLoginAttempts += 1;
                            user.IsLocked = true;
                            db.SaveChanges();
                            goto LOCKED_OUT;

                        case SignInStatus.RequiresVerification:
                            return RedirectToAction( "SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe } );

                        case SignInStatus.Failure:
                            user.InvalidLoginAttempts += 1;
                            db.SaveChanges();
                            break;

                    }

                }

            }

            LOGIN_ERROR:
            model.EnableAnimation = false;
            return ViewWithNotification( model, "Invalid login attempt.  The username or password are incorrect.", NotificationType.Danger );

            LOCKED_OUT:
            model.EnableAnimation = false;
            return ViewWithNotification( model, "Your account has been locked due to too many failed login attempts.  To gain access to the system you will need to contact your system administrator and request that they unlock your account.", NotificationType.Danger );
        
        }

        /// <summary>
        /// Log the user off and redirect them to the login page
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route( "logoff" )]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut( DefaultAuthenticationTypes.ApplicationCookie );
            UserSession.Logoff();
            return Redirect( "~/login" );
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