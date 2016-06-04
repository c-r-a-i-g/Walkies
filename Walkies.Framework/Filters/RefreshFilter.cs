using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;

using Walkies;
using Walkies.Framework.Cookies;
using Walkies.Framework.Web.Session;

namespace Walkies.Framework.Filters
{

    /// <summary>
    /// A filter that uses a cookie with the current URL in it to determine if the page is being refreshed
    /// </summary>
    public class RefreshFilter : IActionFilter
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private bool _isRefresh = false;
        private WalkiesCookie _cookie = null;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Compare the stored url in the cookie with current url, if they are the same and it's a get request, then
        /// its a refresh.
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting( ActionExecutingContext filterContext )
        {

            _cookie = WalkiesCookie.Current;

            if( filterContext.RequestContext.HttpContext.Request.HttpMethod.ToLower() == "post" )
            {
                return;
            }

            var url = this.GetRefreshUrl( filterContext.HttpContext.Request );
            _isRefresh = string.IsNullOrEmpty( _cookie.RefreshUrl )
                      || _cookie.RefreshUrl == url;

            HttpContext.Current.Items[ "IsRefreshed" ] = _isRefresh;

            // Perform actions based on the current request being a refresh action
            var isRefreshedAlready = ( HttpContext.Current.Items[ "isUserSessionRecreated" ] != null );

            if( _isRefresh && isRefreshedAlready == false )
            {
                UserSession.Refresh();
            }

            // Otherwise if the previous request was a refresh, mark the cookie as no longer being a refresh
            else if( _cookie.IsRefresh )
            {
                _cookie.IsRefresh = false;
                _cookie.Save();
            }


        }

        /// <summary>
        /// Store the url in the cookie so that it can be compared with the current url on the next request
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted( ActionExecutedContext filterContext )
        {
            var url = this.GetRefreshUrl( filterContext.HttpContext.Request );
            _cookie = WalkiesCookie.Current;
            _cookie.RefreshUrl = url;
            _cookie.Save();
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
        /// Gets the url that should be used as the breadcrumb url.  If the request header has a rewrite url in it
        /// then this will be used instead of the absolute path of the request Uri
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetRefreshUrl( HttpRequestBase request )
        {

            if( string.IsNullOrEmpty( request.RewriteUrl() ) == false )
            {
                if( request.RewriteUrl() == "/" )
                {
                    return request.Url.ToString().Replace( request.Url.PathAndQuery, "" ) + "/";
                }
                return request.RewriteUrl();
            }

            if( string.IsNullOrEmpty( request.Url.Query ) )
            {
                return request.Url.ToString();
            }

            return request.Url.ToString().Replace( request.Url.Query, "" );

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
