using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Walkies.Core.Enumerations;
using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Attributes
{

    [AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
    public class PermissionAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private readonly List<Permission> _permissions;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public PermissionAttribute( params Permission[] permissions ) 
        {
            _permissions = permissions.ToList();
            this.Order = 0;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Adds one or more required permissions to the context, which will be checked by the AuthorisationFilter.  The
        /// user must have one or more of the specified permissions to be able to access the resource context.
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization( AuthorizationContext filterContext )
        {
            var requiredPermissions = filterContext.RequestContext.RouteData.DataTokens[ "required-permissions" ] as List<Permission>;
            if( requiredPermissions == null ) requiredPermissions = new List<Permission>();
            requiredPermissions.AddRange( _permissions );
            filterContext.RequestContext.RouteData.DataTokens[ "required-permissions" ] = requiredPermissions;
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
