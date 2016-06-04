using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Walkies.Framework.Web.HttpModules
{
    public class SignalRFormsAuthenticationCleanerModule : IHttpModule
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
        /// Removes the pre-send request headers event handler.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Adds the pre-send request headers event handler.
        /// </summary>
        /// <param name="context">The application context</param>
        public void Init( HttpApplication context )
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        /// <summary>
        /// Handles removing the authentication cookie from the specified requests if applicable.
        /// </summary>
        protected void OnPreSendRequestHeaders( object sender, EventArgs e )
        {
            var httpContext = ( (HttpApplication)sender ).Context;

            if ( ShouldCleanResponse( httpContext.Request.Path ) )
            {
                // Remove Auth Cookie from response
                httpContext.Response.Cookies.Remove( FormsAuthentication.FormsCookieName );
                return;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Determines if the specified URL needs to be cleaned of authentication requests.
        /// </summary>
        /// <param name="path">The current URL path</param>
        private bool ShouldCleanResponse( string path )
        {
            path = path.ToLower();
            var urlsToClean = new string[] { "/signalr/" };

            // Check for a Url match
            foreach ( var url in urlsToClean )
            {
                var result = path.IndexOf( url, StringComparison.OrdinalIgnoreCase ) > -1;
                if ( result )
                    return true;
            }

            return false;
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
