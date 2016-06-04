using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Walkies.BaseClasses;

namespace Walkies.Framework.BaseClasses
{
    public class WebViewPageBase<TModel> : BootstrapWebViewPage<TModel>
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public WebViewPageBase()
        {
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Page Actions

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Ajax Actions

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Events

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Returns the html conditionally if the page request is by ajax
        /// </summary>
        /// <param name="html"></param>
        /// <param name="alternateHtml"></param>
        /// <returns></returns>
        public MvcHtmlString IfAjax( string html, string alternateHtml = "" )
        {
            if( this.IsAjax )
            {
                return new MvcHtmlString( html );
            }
            return new MvcHtmlString( alternateHtml );
        }

        /// <summary>
        /// Returns the html conditionally if the page request is an initial page load or a page refresh action
        /// </summary>
        /// <param name="html"></param>
        /// <param name="alternateHtml"></param>
        /// <returns></returns>
        public MvcHtmlString IfRefresh( string html, string alternateHtml = "" )
        {
            if( this.IsAjax == false )
            {
                return new MvcHtmlString( html );
            }
            return new MvcHtmlString( alternateHtml );
        }

        /// <summary>
        /// Returns the html conditionally based on the specified condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="html"></param>
        /// <param name="alternateHtml"></param>
        /// <returns></returns>
        public MvcHtmlString If( bool condition, string html, string alternateHtml = "" )
        {
            if( condition )
            {
                return new MvcHtmlString( html );
            }
            return new MvcHtmlString( alternateHtml );
        }

        /// <summary>
        /// Returns the html conditionally based on the specified condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="html"></param>
        /// <param name="alternateHtml"></param>
        /// <returns></returns>
        public MvcHtmlString IfNot( bool condition, string html, string alternateHtml = "" )
        {
            return If( condition == false, html, alternateHtml );
        }

        /// <summary>
        /// Gets the page key, which is a string that is made up of the controller and the action
        /// </summary>
        public string PageKey
        {
            get
            {
                var controller = ViewContext.RouteData.Values[ "controller" ] as string ?? "Home";
                var action = ViewContext.RouteData.Values[ "action" ] as string ?? "Index";
                var page = ( controller + ":" + action ).ToLower();
                return PageKey;
            }
        }

        /// <summary>
        /// Gets the controller that is controlling this page
        /// </summary>
        public string PageController
        {
            get
            {
                return ( ViewContext.RouteData.Values[ "controller" ] as string ?? "Home" ).ToLower();
            }
        }

        /// <summary>
        /// Gets the action that is controlling this page
        /// </summary>
        public string PageAction
        {
            get
            {
                return ( ViewContext.RouteData.Values[ "action" ] as string ?? "Home" ).ToLower();
            }
        }

        /// <summary>
        /// Gets the path of the page
        /// </summary>
        public string Path
        {
            get
            {
                return ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath.ToLower();
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        /// <summary>
        /// Returns true if this is a PDF request
        /// </summary>
        public bool IsPdf
        {
            get
            {
                return string.IsNullOrEmpty( Request.Headers[ "pdf-request" ] ) == false;
            }
        }

        /// <summary>
        /// Returns true if the page has been requested with copy=true in the querystring
        /// </summary>
        public bool IsCopy
        {
            get
            {
                return Request.QueryString[ "copy" ] == "true";
            }
        }

        /// <summary>
        /// Returns true if the page is a full refresh, i.e. not an ajax request
        /// </summary>
        public bool IsRefresh
        {
            get
            {
                return this.IsAjax == false;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}