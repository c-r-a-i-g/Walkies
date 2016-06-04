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
using System.Text.RegularExpressions;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Walkies.Framework.Web.Session;
using Walkies.Framework.Interfaces;
using Walkies.Core.Enumerations;


namespace Walkies.Framework.Filters
{

    /// <summary>
    /// A filter that logs an event to the debug window that shows when a page request, ajax page request or
    /// plain ajax request comes through
    /// </summary>
    public class PageEventLogFilter : ActionFilterAttribute
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public PageEventLogFilter()
        {

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Logs the page's post/get state and whether or not it is an ajax call or full request
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting( ActionExecutingContext filterContext )
        {

            var output = "";
            var path = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
            var isAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            var method = filterContext.HttpContext.Request.HttpMethod.ToUpper();

            // Remove the javascript cache killer if there is one
            var regex = new Regex( @"_=[\d]+" );
            path = regex.Replace( path, "" ).TrimEnd( '&','?' );

            if( isAjax == false || filterContext.HttpContext.Request.Headers.AllKeys.Contains( "X-Walkies-PAGE" ) )
            {
                output = "## " + method + ( isAjax ? " PAGE (ajax) " : " PAGE " ) + path;
            }

            else
            {
                output = " # " + method + " DATA (ajax) " + path;
            }

            Debug.Print( "" );
            Debug.Print( output );
            Debug.Print( new String( '-', output.Length ) );

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
