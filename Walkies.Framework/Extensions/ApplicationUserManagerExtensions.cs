using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Database;
using Walkies.Framework.Models.Admin;
using Walkies.Framework.Web.Authorisation;
using Walkies.Framework.Enumerations;
using Walkies.Core.Enumerations;
using Walkies.Database.Entities;

namespace Walkies.Framework
{
    public static class ApplicationUserManagerExtensions
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
        /// Generates a unique username using the first name, last name, email and taking into account the 
        /// existing usernames in the membership table.  If the UsernameFormat of the client is using EmailAddress,
        /// and the email address already exists in the database, then the username will be returned empty.  This
        /// condition must be tested for in the calling function.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="model"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GenerateMembershipUsername( this ApplicationUserManager userManager, UserModel model, WalkiesDB db )
        {

            var usernameFormat = UsernameFormat.EmailAddress;
            var userName = "";

            // Create a desired username using the email or combinations of the first/last names, depending
            // on the settings of the domain
            if( usernameFormat == UsernameFormat.EmailAddress )
            {
                userName = model.Email;
            }

            //else if( domainSettings.UsernameFormat == UsernameFormat.FirstNameAndLastName )
            //{
            //    userName = model.FirstName.ToLower().Replace( " ", string.Empty ) + model.LastName.ToLower().ReplaceAny( new string[] { " ", "'", "-" }, string.Empty );
            //}

            //else if( domainSettings.UsernameFormat == UsernameFormat.FirstInitialAndLastName )
            //{
            //    userName = model.FirstName.Substring( 0, 1 ).ToLower().Replace( " ", string.Empty ) + model.LastName.ToLower().ReplaceAny( new string[] { " ", "'", "-" }, string.Empty );
            //}

            // Get a count of users that have the desired username, hopefully it will be zero.
            var userCount = db.Users.Count( u => u.UserName == userName );

            // Were using a generated username and the there is already a matching username in the database.  Because
            // its a generated name, we can simply generate another unique username.  This is guaranteed to result
            // in a valid username that is unique
            if( userCount != 0 && usernameFormat != UsernameFormat.EmailAddress )
            {

                int n = 1;
                userCount = db.Users.Count( u => u.UserName == userName + n.ToString() );

                while( userCount != 0 )
                {
                    n += 1;
                    userCount = db.Users.Count( u => u.UserName == userName + n.ToString() );
                }

                userName = userName + n.ToString();

            }

            // The email address already exists in the database, so we have no choice but to reject it
            else if( userCount != 0 && usernameFormat == UsernameFormat.EmailAddress )
            {
                userName = "";
            }

            return userName;

        }

        /// <summary>
        /// Attempts to create an application user and returns true if the operation was successful
        /// </summary>
        /// <param name="model"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SaveUserResult CreateApplicationUser( this ApplicationUserManager userManager, UserModel model, string password )
        {

            /* Generate a username. If this function returns an empty string, then it means
             * we are using email address and the email address is already being used */
            var db = new WalkiesDB();
            //var client = db.Clients.FirstOrDefault( g => g.ClientId == model.ClientId );
            string userName = userManager.GenerateMembershipUsername( model, db );

            if( string.IsNullOrEmpty( userName ) )
            {
                return new SaveUserResult( null, userName, CreateApplicationUserResult.EmailAddressAlreadyExists );
            }

            try
            {
                var applicationUser = new ApplicationUser { UserName = userName, Email = model.Email };

                var result = Task.Run( () =>
                {
                    return userManager.CreateAsync( applicationUser, password );
                } );

                if ( result.Result.Succeeded == false )
                {
                    return new SaveUserResult( null, userName, CreateApplicationUserResult.Failed );
                }

                var userId = Guid.Parse( applicationUser.Id );
                return new SaveUserResult( userId, userName, CreateApplicationUserResult.Successful );
            }

            catch( Exception )
            {
                return new SaveUserResult( null, userName, CreateApplicationUserResult.Failed );

            }

        }

        /// <summary>
        /// Attempts to delete an application user 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static void DeleteApplicationUser( this ApplicationUserManager userManager, string userName )
        {
            try
            {
                var user = userManager.Users.FirstOrDefault( u => u.UserName == userName );
                userManager.DeleteAsync( user );
            }
            catch( Exception ) { }
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
