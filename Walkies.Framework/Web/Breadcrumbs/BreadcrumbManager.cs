using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Walkies;
using Walkies.Framework.Cookies;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Session;

namespace Walkies.Framework.Breadcrumbs
{
    public class BreadcrumbManager
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// Called by the PageModelBase when the page title is set (usually from the view itself), and attempts
        /// to set the title of the breadcrumb to the pages' title.
        /// </summary>
        /// <param name="pageTitle"></param>
        public static void AddBreadcrumb( IPageModel model )
        {

            var cookie = WalkiesCookie.Current;
            var url = BreadcrumbManager.GetBreadcrumbUrl( HttpContext.Current.Request );
            var breadcrumb = new Breadcrumb();

            var result = new List<Breadcrumb>();
            result.Add( breadcrumb );

            breadcrumb.Text = model.PageTitle;
            breadcrumb.Url = url;

            foreach( var item in cookie.Breadcrumbs.Where( x => x.Url != "/login" ).DistinctBy( b => b.Url ) )
            {
                if( url.StartsWith( item.Url ) && url != item.Url )
                {
                    result.Add( item );
                }
            }

            cookie.Breadcrumbs = result.OrderByDescending( b => b.Url ).ToList();
            cookie.Save();

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Gets the url that should be used as the breadcrumb url.  If the request header has a rewrite url in it
        /// then this will be used instead of the absolute path of the request Uri
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetBreadcrumbUrl( HttpRequest request )
        {
            if( string.IsNullOrEmpty( request.RewriteUrl() ) == false )
            {
                return request.RewriteUrl();
            }
            return HttpContext.Current.Request.Url.AbsolutePath;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets the back url or the url of the homepage
        /// </summary>
        public static string BackUrl
        {
            get
            {
                if( UserSession.Current.IsAuthenticated == false && WalkiesCookie.Current.Breadcrumbs.Count < 2 )
                {
                    Debug.Print( "-- User is not authenticated, using '/login' as BackUrl" );
                    return "/login";
                }

                return WalkiesCookie.Current.Breadcrumbs.Count < 2 ? "/" : WalkiesCookie.Current.Breadcrumbs[ 1 ].Url;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
