﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Walkies.Framework.Web.Authorisation
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager( ApplicationUserManager userManager, IAuthenticationManager authenticationManager )
            : base( userManager, authenticationManager )
        {
             
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync( ApplicationUser user )
        {
            return user.GenerateUserIdentityAsync( (ApplicationUserManager)UserManager, DefaultAuthenticationTypes.ApplicationCookie );
        }

        public static ApplicationSignInManager Create( IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context )
        {
            return new ApplicationSignInManager( context.GetUserManager<ApplicationUserManager>(), context.Authentication );
        }
    }
}
