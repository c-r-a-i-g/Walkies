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
using Walkies.Framework.Notifications;
using Newtonsoft.Json;
using Walkies.Database.Entities;
using Walkies.Framework.Models.Emails;
using Walkies.Framework.Attributes;

namespace Walkies.Framework.Web.Authorisation.Controllers
{
    [Authorize]
    public partial class AccountController : WebControllerBase
    {

        #region Helpers
        
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
        
        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public AccountController()
        {
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode( string provider, string returnUrl, bool rememberMe )
        {
            // Require that the user has already logged in via username/password or external login
            if ( !await SignInManager.HasBeenVerifiedAsync() )
            {
                return View( "Error" );
            }
            return View( new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe } );
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode( VerifyCodeViewModel model )
        {
            if ( !ModelState.IsValid )
            {
                return View( model );
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync( model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser );
            switch ( result )
            {
                case SignInStatus.Success:
                    return RedirectToLocal( model.ReturnUrl );
                case SignInStatus.LockedOut:
                    return View( "Lockout" );
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError( "", "Invalid code." );
                    return View( model );
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        [Route( "register" )]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route( "register" )]
        public async Task<ActionResult> Register( RegisterViewModel model )
        {
            if ( ModelState.IsValid )
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync( user, model.Password );
                if ( result.Succeeded )
                {



                    await SignInManager.SignInAsync( user, isPersistent: false, rememberBrowser: false );

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return Redirect( "/" );
                }
                AddErrors( result );
            }

            // If we got this far, something failed, redisplay form
            return View( model );
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail( string userId, string code )
        {
            if ( userId == null || code == null )
            {
                return View( "Error" );
            }
            var result = await UserManager.ConfirmEmailAsync( userId, code );
            return View( result.Succeeded ? "ConfirmEmail" : "Error" );
        }

        [HttpPost]
        [Route( "account/change-theme" )]
        public ActionResult ChangeTheme( string theme )
        {
            var userManager = new Managers.UserManager();

            var result = userManager.ChangeTheme( UserSession.Current.UserId.Value, theme );

            if ( result.IsSuccessful )
            {
                UserSession.Current.ChangeTheme( theme );
            }

            return JsonSaveResult( result, "Theme Change" );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route( "change-password" )]
        public ActionResult ChangePassword( ChangePasswordPageModel model )
        {
            Notification notification = new Notification()
            {
                NotificationType = NotificationType.Danger
            };

            if ( model.NewPassword == model.ConfirmPassword )
            {
                IdentityResult result = SignInManager.UserManager.ChangePassword( model.UserId.ToString(), model.OldPassword, model.NewPassword );

                if ( result.Succeeded )
                {
                    return RedirectToLocal( model.ReturnUrl );
                }
                else
                {
                    notification.Content = string.Join( ". ", result.Errors );
                }
            }
            else
            {
                notification.Content = "The entered in passwords do not match. Please try again";
            }

            var properties = new
            {
                notification = notification
            };

            return Content( JsonConvert.SerializeObject( properties ), ApplicationJson );
        }

        //
        // GET: /forgotten-password
        [AllowAnonymous]
        [Route( "forgotten-password" )]
        public ActionResult ForgotPassword()
        {
            var model = new ForgottenPasswordModel();
            return View( "~/Views/Modals/ForgottenPassword.cshtml", model );
        }

        //
        // POST: /forgotten-password
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route( "forgotten-password" )]
        public async Task<ActionResult> ForgotPassword( ForgottenPasswordModel model )
        {
            Notification notification = new Notification()
            {
                NotificationType = NotificationType.Danger,
                Content = "An email could not be sent, please contact support"
            };

            bool wasEmailSent = false;

            if ( ModelState.IsValid )
            {
                var user = await UserManager.FindByNameAsync( model.UserName );
                if ( user == null )
                {
                    notification = new Notification()
                    {
                        NotificationType = NotificationType.Warning,
                        Content = "Your email doesn't exist. Please contact support or request an account"
                    };
                }
                else
                {
                    try
                    {
                        string code = await UserManager.GeneratePasswordResetTokenAsync( user.Id );
                        var callbackUrl = Url.Action( "ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme );
                        PasswordResetRequestEmailModel passwordResetModel = new PasswordResetRequestEmailModel( Guid.Parse( user.Id ), callbackUrl );

                        passwordResetModel.Send();

                        notification = new Notification()
                        {
                            NotificationType = NotificationType.Success,
                            Content = "An email has been sent"
                        };

                        wasEmailSent = true;
                    }
                    catch ( Exception )
                    {
                        notification = new Notification()
                        {
                            NotificationType = NotificationType.Danger,
                            Content = "An error occurred, please try again or contact support"
                        };
                    }
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            var properties = new
            {
                sent = wasEmailSent,
                notification = notification
            };

            return Content( JsonConvert.SerializeObject( properties ), ApplicationJson );
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        [Route( "reset-password" )]
        public ActionResult ResetPassword( string token )
        {
            return token == null ? View( "Error" ) : View( new ResetPasswordModel( token ) );
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route( "reset-password" )]
        public async Task<ActionResult> ResetPassword( ResetPasswordViewModel model )
        {
            if ( !ModelState.IsValid )
            {
                return View( model );
            }
            var user = await UserManager.FindByNameAsync( model.Email );
            if ( user == null )
            {
                // Don't reveal that the user does not exist
                return RedirectToAction( "ResetPasswordConfirmation", "Account" );
            }
            var result = await UserManager.ResetPasswordAsync( user.Id, model.Code, model.Password );
            if ( result.Succeeded )
            {
                return RedirectToAction( "ResetPasswordConfirmation", "Account" );
            }
            AddErrors( result );
            return View( model );
        }

        [HttpPost]
        [Route( "account/reset-account/{userId:Guid}/{resetPassword:bool}" )]
        [AuthoriseWith( typeof( Managers.UserManager ) )]
        public ActionResult ResetAccount( Guid userId, bool resetPassword )
        {
            var manager = new Managers.UserManager();
            var result = manager.ResetAccount( userId, resetPassword );

            if ( result.IsSuccessful && resetPassword )
            {
                return JsonNotification( "The user has been sent an email containing a link that will allow them to create a new password.", NotificationType.Success, new { success = true } );
            }

            if ( result.IsSuccessful && resetPassword == false )
            {
                return JsonNotification( "The lock on the users account has been removed.  They have been sent an email confirming that their account has been unlocked.", NotificationType.Success, new { success = true } );
            }

            return JsonNotification( "The user account could not be reset.  Please try again, and if the problem persists then contact the support team.", NotificationType.Danger, new { success = false } );

        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin( string provider, string returnUrl )
        {
            // Request a redirect to the external login provider
            return new ChallengeResult( provider, Url.Action( "ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl } ) );
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode( string returnUrl, bool rememberMe )
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if ( userId == null )
            {
                return View( "Error" );
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync( userId );
            var factorOptions = userFactors.Select( purpose => new SelectListItem { Text = purpose, Value = purpose } ).ToList();
            return View( new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe } );
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode( SendCodeViewModel model )
        {
            if ( !ModelState.IsValid )
            {
                return View();
            }

            // Generate the token and send it
            if ( !await SignInManager.SendTwoFactorCodeAsync( model.SelectedProvider ) )
            {
                return View( "Error" );
            }
            return RedirectToAction( "VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe } );
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback( string returnUrl )
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if ( loginInfo == null )
            {
                return RedirectToAction( "Login" );
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync( loginInfo, isPersistent: false );
            switch ( result )
            {
                case SignInStatus.Success:
                    return RedirectToLocal( returnUrl );
                case SignInStatus.LockedOut:
                    return View( "Lockout" );
                case SignInStatus.RequiresVerification:
                    return RedirectToAction( "SendCode", new { ReturnUrl = returnUrl, RememberMe = false } );
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View( "ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email } );
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation( ExternalLoginConfirmationViewModel model, string returnUrl )
        {
            if ( User.Identity.IsAuthenticated )
            {
                return RedirectToAction( "Index", "Manage" );
            }

            if ( ModelState.IsValid )
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if ( info == null )
                {
                    return View( "ExternalLoginFailure" );
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync( user );
                if ( result.Succeeded )
                {
                    result = await UserManager.AddLoginAsync( user.Id, info.Login );
                    if ( result.Succeeded )
                    {
                        await SignInManager.SignInAsync( user, isPersistent: false, rememberBrowser: false );
                        return RedirectToLocal( returnUrl );
                    }
                }
                AddErrors( result );
            }

            ViewBag.ReturnUrl = returnUrl;
            return View( model );
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( _userManager != null )
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if ( _signInManager != null )
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose( disposing );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        private void AddErrors( IdentityResult result )
        {
            foreach ( var error in result.Errors )
            {
                ModelState.AddModelError( "", error );
            }
        }

        private ActionResult RedirectToLocal( string returnUrl )
        {
            returnUrl = returnUrl ?? "~/";

            if ( Url.IsLocalUrl( returnUrl ) )
            {
                return Redirect( returnUrl );
            }

            return Redirect( "~/" );

        }

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