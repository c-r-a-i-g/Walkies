using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Attributes
{

    [AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
    public class AuthoriseWithAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private Type[] _authorisors = null;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public AuthoriseWithAttribute( params Type[] authorisors ) 
        {

            foreach( Type type in authorisors )
            {
                if( typeof( IAuthorisor ).IsAssignableFrom( type ) == false )
                {
                    throw new ArgumentException( "One or more of the authorisors specified in the 'authorisors' argument is not a type of IAuthorisor.  The authorisors specified must implement the IAuthorisor interface." );
                }
            }

            _authorisors = authorisors;
            this.Order = 1;

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
            if( _authorisors == null ) return;
            filterContext.RequestContext.RouteData.DataTokens[ "authorisors" ] = _authorisors;
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
