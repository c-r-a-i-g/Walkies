using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Diagnostics;
using System.IO;

using Walkies;
using Walkies.Framework.BaseClasses;
using Walkies.Framework.Models.Admin;
using Walkies.Database.Entities;
using Walkies.Framework.Web.Authorisation;
using Walkies.Framework.Web;
using Walkies.Framework.Web.Authorisation.Models;
using Walkies.Database;
using Walkies.Framework.Models.Modals;
using Walkies.Framework.Enumerations;
using Walkies.Framework.Models.Emails;

using Walkies.Framework.Web.Session;
using Walkies.Framework.Cookies;
using Walkies.Core.Configuration;
using Walkies.Core.Enumerations;
using Walkies.Database.JsonProperties;

namespace Walkies.Framework.Managers
{

    public class UserManager : DataManagerBase<UserManager, UserModel, User, Guid>
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private static ApplicationUserManager _userManager;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Determines if the user can access the entity specified in the authorisation context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanAccess( AuthorizationContext context )
        {

            if( UserSession.Can( Permission.Administrator ) ) return true;

            Guid? userId = context.GetRouteData<Guid>( "userId" );

            if( userId != null )
            {
                // Validate that the user is allowed to manage the entity
            }

            return true;

        }

        /// <summary>
        /// Occurs when a user is created, allows us to get the ApplicationUserId and set the UserId of the
        /// new user to this value
        /// </summary>
        /// <param name="entity"></param>
        public override void OnAfterCreateEntity( User entity )
        {

            if( this.Model.ApplicationUserId.HasValue == false )
            {
                throw new ApplicationException( "ApplicationUserId does not have a value" );
            }

            entity.UserId = this.Model.ApplicationUserId.Value;

        }

        /// <summary>
        /// Updates the users account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public SaveState UpdateMyAccount( MyAccountModel model )
        //{

        //    try
        //    {
        //        var user = _db.Users.Find( model.UserId );
        //        if( user == null ) return SaveState.Error;

        //        AutoMap.Map( model, user );
        //        _db.SaveChanges();

        //        var result = SaveState.Success( user.UserId );
        //        result.Entity = user;
        //        return result;
        //    }

        //    catch( Exception )
        //    {
        //        return SaveState.Error;
        //    }

        //}

        /// <summary>
        /// Updates the users account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SaveState SaveAcceptTerms( Guid userId, LoginModel model )
        {

            try
            {
                var user = _db.Users.Find( userId );
                if( user == null )
                {
                    var errorResult = SaveState.Error;
                    errorResult.Data = "There was a problem updating the password, the password has not been changed.  Please try again, and if the problem persists then contact the support team";
                    return errorResult;
                }

                // Change their password
                var applicationUser = UserManager.Authentication.FindById( user.UserId.ToString() );
                var authResult = UserManager.Authentication.ChangePassword( user.UserId.ToString(), model.OldPassword, model.NewPassword );

                if( authResult.Succeeded == false )
                {

                    var errorResult = SaveState.Error;
                    if( authResult.Errors.Contains( "Incorrect password." ) )
                    {
                        errorResult.Data = "The old password was entered incorrectly.  The password has not been changed.";
                    }

                    else
                    {
                        errorResult.Data = string.Format( "There was a problem updating the password. {0} The password has not been changed.", authResult.Errors.First() );
                    }
                    return errorResult;
                }

                user.DateAgreedTerms = DateTime.Now;
                _db.SaveChanges();

                WalkiesCookie.Current.UserSession.HasAgreedToTerms = true;
                WalkiesCookie.Current.Save();

                UserManager.Authentication.Update( applicationUser );

                var result = SaveState.Success( user.UserId );
                result.Entity = user;
                return result;
            }

            catch( Exception )
            {
                return SaveState.Error;
            }

        }
        /// <summary>
        /// Resets the users password and sends them an email confirming the temporary password that they have been issued
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SaveState ResetAccount( Guid userId, bool resetPassword )
        {

            try
            {

                var user = _db.Users.Find( userId );
                if( user == null ) return SaveState.Error;

                user.InvalidLoginAttempts = 0;
                user.IsLocked = false;
                _db.SaveChanges();
                
                var applicationUser = UserManager.Authentication.FindById( user.UserId.ToString() );
                
                applicationUser.LockoutEndDateUtc = null;
                applicationUser.AccessFailedCount = 0;
                UserManager.Authentication.Update( applicationUser );
                
                if( resetPassword )
                {
                    var code = UserManager.Authentication.GeneratePasswordResetToken( applicationUser.Id );
                    var callbackUrl = "/reset-password?token=" + HttpContext.Current.Server.UrlEncode( code );
                    var email = new PasswordResetRequestEmailModel( user.UserId, callbackUrl );
                    email.SendTo( user );
                }

                else
                {
                    var email = new AccountResetEmailModel( user.UserId );
                    email.SendTo( user );
                }

                var result = SaveState.Success( user.UserId );
                result.Entity = user;
                return result;

            }

            catch( Exception )
            {
                return SaveState.Error;
            }

        }

        /// <summary>
        /// Changes the theme for the specified user.
        /// </summary>
        /// <param name="userId">The user to change the theme for</param>
        /// <param name="theme">The new theme for the user</param>
        /// <returns></returns>
        public SaveState ChangeTheme( Guid userId, string theme )
        {
            try
            {
                var user = _db.Users.Find( userId );

                user.Properties.Theme = theme;

                _db.SaveChanges();

                return SaveState.Success( userId );
            }
            catch( Exception )
            {
                return SaveState.Error;
            }
        }

        /// <summary>
        /// Changes the users password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SaveState ChangePassword( ControllerContext context, ChangePasswordModel model )
        {

            try
            {

                var user = _db.Users.Find( model.UserId );
                if( user == null ) return SaveState.Error;

                var applicationUser = UserManager.Authentication.FindById( user.UserId.ToString() );

                // Try to change the password
                var authResult = UserManager.Authentication.ChangePassword( user.UserId.ToString(), model.OldPassword, model.NewPassword );

                if( authResult.Succeeded == false )
                {
                    var errorResult = SaveState.Error;
                    if( authResult.Errors.Contains( "Incorrect password." ) )
                    {
                        errorResult.Data = "The old password was entered incorrectly.  The password has not been changed.";
                    }

                    else
                    {
                        errorResult.Data = string.Format( "There was a problem updating the password. {0} The password has not been changed.", authResult.Errors.First() );
                    }
                    return errorResult;
                }

                UserManager.Authentication.Update( applicationUser );

                // TODO: Send email notifying that password has been changed

                var result = SaveState.Success( user.UserId );
                result.Entity = user;
                return result;

            }

            catch( Exception )
            {
                var errorResult = SaveState.Error;
                errorResult.Data = "There was a problem updating the password. The password has not been changed.  Please try again and if the problem persists then contact the support team.";
                return errorResult;
            }

        }

        /// <summary>
        /// Primary function for creating a new user.  This function will create the application user, the user profile and
        /// also send the welcome email.  If the user can not be created for any reason, the SaveState will indicate the issue.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public SaveState CreateUser( string firstName, string lastName, string email, string roleName )
        {
            var model = new UserModel();
            model.FirstName = firstName;
            model.LastName = lastName;
            model.Email = email;

            return this.CreateUser( model );
        }

        /// <summary>
        /// Primary function for creating a new user.  This function will create the application user, the user profile and
        /// also send the welcome email.  If the user can not be created for any reason, the SaveState will indicate the issue.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SaveState CreateUser( UserModel model )
        {

            var result = SaveState.Error;
            result.Entity = model;

            if( string.IsNullOrEmpty( model.FirstName.Trim() ) || string.IsNullOrEmpty( model.LastName.Trim() ) )
            {
                result.Data = "The first name and/or last name do not have a value";
                return result;
            }

            else if( string.IsNullOrEmpty( model.Email.Trim() ) )
            {
                result.Data = "The email does not have a value";
                return result;
            }

            else if( model.RoleId == Guid.Empty )
            {
                result.Data = "The role does not have a valid value";
                return result;
            }

            // Create the application user
            var password = PasswordGenerator.Generate( ApplicationSettings.Current.Passwords );
            var appUser = UserManager.Authentication.CreateApplicationUser( model, password );
            
            if( appUser.State == CreateApplicationUserResult.Successful )
            {

                model.UserName = appUser.Username;
                model.ApplicationUserId = appUser.UserId;

                // The application user was created successfully, create the user profile.
                result = model.Save();
                if( result.IsSuccessful )
                {
                    // The user profile was created successfully, send the welcome email and return the result
                    var user = result.Entity as User;
                    WelcomeEmailModel email = new WelcomeEmailModel( user.UserId, appUser.Username, password, appUser.Username == model.Email );
                    email.SendTo( user );
                    return result;
                }

                /* The save of the user failed but an applcation user was created, it will need to be deleted */
                else
                {
                    UserManager.Authentication.DeleteApplicationUser( appUser.Username );
                }
            }

            /* The client is authenticating with email address and there is already a user with email address provided */
            else if( appUser.State == CreateApplicationUserResult.EmailAddressAlreadyExists )
            {
                return SaveState.AlreadyExists;
            }

            return SaveState.Error;

        }

        /// <summary>
        /// Resets the password and sends a welcome email to the specified userId
        /// </summary>
        /// <param name="userId"></param>
        public SaveState ResetPasswordAndSendWelcomeEmail( Guid userId )
        {

            try
            {
                var db = new WalkiesDB();
                var user = db.Users.Find( userId );

                // Create a new password and find the app user
                var password = PasswordGenerator.Generate( ApplicationSettings.Current.Passwords );
                var appUser = UserManager.Authentication.FindById( userId.ToString() );

                // Try to change the password
                var resetToken = UserManager.Authentication.GeneratePasswordResetToken( userId.ToString() );
                var authResult = UserManager.Authentication.ResetPassword( user.UserId.ToString(), resetToken, password );

                if( authResult.Succeeded == false )
                {
                    var errorResult = SaveState.Error;
                    errorResult.Data = string.Format( "There was a problem updating the password. {0} The password has not been changed.", authResult.Errors.First() );
                    return errorResult;
                }

                // Reset the app user and update it
                appUser.AccessFailedCount = 0;
                appUser.LockoutEndDateUtc = null;
                UserManager.Authentication.Update( appUser );

                // Reset any violations and save the user
                user.InvalidLoginAttempts = 0;
                user.IsLocked = false;
                user.DateAgreedTerms = null; // Forces the user to create a new password on login
                db.SaveChanges();

                // Send the welcome email
                WelcomeEmailModel email = new WelcomeEmailModel( user.UserId, user.UserName, password, user.UserName == user.Email );
                email.SendTo( user );

                return SaveState.Successful;

            }

            catch( Exception )
            {
                return SaveState.Error;
            }

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        /// <summary>
        /// Occurs after the newEntity is saved
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="saveState"></param>
        /// <param name="controllerContext"></param>
        /// <param name="saveContext"></param>
        public override void OnAfterSaveEntity( User entity, SaveState saveState, SaveContext saveContext )
        {

            if( saveState.IsSuccessful == false ) return;

            /* Temporarily commented out - previously any role permissions were added to the user as
             * user permissions, but now they are inferred from the role permission and combined with
             * any user permissions that are manually applied - ie all userpriviledges are now
             * considered to be manually applied.
             * 
             * Because of this, there should not be a need to create user permissions from the selected
             * role, because the role permissions will be granted to the user when they login anyway
             * through the user.GetEffectivePriviledges() function.
             */
            //this.UpdatePermissions( entity );

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Updates the users permissions
        /// </summary>
        /// <param name="newEntity"></param>
        private void UpdatePermissions( User entity )
        {

            //var rolePriviledges = _db.RolePriviledges.Where( rt => rt.RoleId == entity.RoleId ).Select( rt => rt.Priviledge );
            //var userPriviledges = _db.UserPriviledges.Where( up => up.UserId == entity.UserId );

            ///* Delete any removed permissions */
            //var removedIds = userPriviledges.Where( p => rolePriviledges.Contains( p.Priviledge ) == false && p.IsManuallyApplied == false )
            //                                .Select( p => p.UserPriviledgeId );

            //if( removedIds.Count() > 0 )
            //{
            //    _db.UserPriviledges.Delete( removedIds );
            //}

            ///* Create any new permissions */
            //if( rolePriviledges != null && rolePriviledges.Count() > 0 )
            //{
            //    var newPermissions = rolePriviledges.Where( p => userPriviledges.Select( up => up.Priviledge ).Contains( p ) == false );
            //    foreach( var permission in newPermissions )
            //    {
            //        var userPriviledge = new UserPriviledge();
            //        userPriviledge.UserId = entity.UserId;
            //        userPriviledge.Priviledge = permission;
            //        userPriviledge.IsManuallyApplied = false;
            //        _db.UserPriviledges.Add( userPriviledge );
            //    }
            //    _db.SaveChanges();
            //}

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets the ApplicationUserManager class registered with the Owin context
        /// </summary>
        public static ApplicationUserManager Authentication
        {
            get
            {
                return _userManager ?? ApplicationUserManager.Current;
            }
            private set
            {
                _userManager = value;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
