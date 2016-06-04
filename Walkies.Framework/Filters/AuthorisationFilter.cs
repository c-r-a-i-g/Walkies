using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Diagnostics;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Walkies.Framework.Web.Session;
using Walkies.Framework.Interfaces;
using Walkies.Core.Enumerations;
using Walkies.Framework.Attributes;

namespace Walkies.Framework.Filters
{

    /// <summary>
    /// A filter that validates that the user has the correct permissions to access a piece of content
    /// </summary>
    public class AuthorisationFilter : AuthorizeAttribute
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public AuthorisationFilter()
        {
            this.Order = 100;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Checks if the user is authorised to perform the action and redirects them to the not authorised page 
        /// if they are not allowed.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization( AuthorizationContext filterContext )
        {

            base.OnAuthorization( filterContext );

            var url = filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath.ToLower();
            var isAuthenticated = filterContext.RequestContext.HttpContext.Request.IsAuthenticated;
            var hasAgreedTerms = UserSession.Current.HasAgreedToTerms;

            if( filterContext.ActionDescriptor.GetCustomAttributes( typeof(AllowAnonymousAttribute), true ).Count() > 0 )
            {
                return;
            }

            // The user is authenticated
            if( isAuthenticated )
            {

                // They are not a website user but they are trying to access a website page
                //if( filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes( typeof( WebsiteUsersAttribute ), true ).Count() > 0 )
                //{
                //    if( UserSession.Can( Permission.AccessWalkiesWebsite ) == false )
                //    {
                //        filterContext.Result = new RedirectResult( "~/error/not-authorised", false );
                //        return;
                //    }
                //}

                // They havent agreed to terms, keep them on the login page
                if( hasAgreedTerms == false && url.StartsWith( "/login" ) == false )
                {
                    filterContext.Result = new RedirectResult( "~/login", false );
                    return;
                }

                // They are logged in and have agreed to terms, dont let them access the login page
                else if( hasAgreedTerms && url.StartsWith( "/login" ) )
                {
                    filterContext.Result = new RedirectResult( "~/", false );
                    return;
                }

                // Validate their permissions to a protected resource
                var hasRequiredPermissions = this.HasRequiredPermissions( filterContext );
                if( hasRequiredPermissions == false )
                {
                    filterContext.Result = new RedirectResult( "~/error/not-authorised", false );
                    return;
                }

                // Validate their ability to access a resource based on authorisor attribute on the 
                // action.  Authorisors are custom classes that can perform authorisation on a resource
                // at run time.
                var canAccessProtectedEntity = this.CanAccessProtectedEntity( filterContext );
                if( canAccessProtectedEntity == false )
                {
                    filterContext.Result = new RedirectResult( "~/error/not-authorised", false );
                    return;
                }
            
            }

        }

        /// <summary>
        /// Handle an unauthorised request
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest( AuthorizationContext filterContext )
        {

            if( filterContext.HttpContext.Request.IsAjaxRequest() )
            {

                Debug.Print( "Unauthorised" );
                Debug.Print( filterContext.RequestContext.HttpContext.Request.Url.ToString() );

                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        IsSessionExpired = true
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest( filterContext );
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

        /// <summary>
        /// Returns true if the context has no required permissions, or if the user has at least one of the required permissions
        /// defined through attributes on the context.
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool HasRequiredPermissions( AuthorizationContext filterContext )
        {
            if( UserSession.Can( Permission.Administrator ) ) return true;
            var permissions = UserSession.Current.Permissions;
            var attributePermissions = filterContext.RequestContext.RouteData.DataTokens[ "required-permissions" ] as List<Permission>;
            var requiredPermissions = attributePermissions ?? new List<Permission>();
            return requiredPermissions.Count == 0 || permissions.Any( p => requiredPermissions.Contains( p ) );
        }

        /// <summary>
        /// If the route data contains an authorisor, i.e. the requested action has the AuthoriseWith attribute,
        /// then try to create an instance of the authorisor and call its "CanAccess" method to determine if the
        /// user is allowed access to the resource.
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool CanAccessProtectedEntity( AuthorizationContext filterContext )
        {

            if( UserSession.Can( Permission.Administrator ) ) return true;

            var authorisors = filterContext.RequestContext.RouteData.DataTokens[ "authorisors" ] as Type[];
            if( authorisors == null || authorisors.Length == 0 ) return true;

            var isAuthorised = false;
            foreach( var authorisor in authorisors )
            {
                var manager = Activator.CreateInstance( authorisor ) as IAuthorisor;
                if( manager != null )
                {
                    isAuthorised = manager.CanAccess( filterContext );
                }

                if( isAuthorised == false ) break;

            }

            return isAuthorised;

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
