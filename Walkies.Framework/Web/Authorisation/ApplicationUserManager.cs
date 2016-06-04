using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Walkies.Core.Configuration;
using Walkies.Framework.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Walkies.Framework.Web.Authorisation
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {

        private static ApplicationUserManager _current = null;

        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var manager = new ApplicationUserManager( new UserStore<ApplicationUser>( context.Get<ApplicationDbContext>() ) );
            
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.MaxFailedAccessAttemptsBeforeLockout = ApplicationSettings.Current.Login.MaxFailedAccessAttemptsBeforeLockout;

            int lockoutTime = ApplicationSettings.Current.Login.DefaultLockoutTime;

            if ( lockoutTime < 0 )
            {
                // Lock the user out for 200 years
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes( 365 * 200 );
            }
            else
            {
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromDays( lockoutTime );
            }

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            _current = manager;
            return manager;

        }

        /// <summary>
        /// Gets the current user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ApplicationUser GetUser( string userName )
        {
            var user = this.FindByName( userName );
            if( user == null ) return null;
            return user;
        }

        public static ApplicationUser CurrentUser
        {
            get
            {
                var manager = new ApplicationUserManager( new UserStore<ApplicationUser>( HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>() ) );
                return manager.GetUser( HttpContext.Current.User.Identity.Name );
            }
        }
        
        /// <summary>
        /// Gets the current ApplicationUserManager
        /// </summary>
        public static ApplicationUserManager Current
        {
            get
            {
                if( _current == null )
                {
                    _current = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _current;
            }
        }

    }
}
