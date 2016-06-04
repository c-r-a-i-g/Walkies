using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Walkies.Framework.Cookies;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Walkies.Framework.Filters
{

    /// <summary>
    /// A filter that ensures that the client has a GiftRewards cookie
    /// </summary>
    public class CookieFilter : IActionFilter
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private HttpCookie _cookie = null;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting( ActionExecutingContext filterContext )
        {

            _cookie = filterContext.HttpContext.Request.Cookies[ WalkiesCookie.CookieName ];
            if( _cookie == null )
            {
                var cookie = new HttpCookie( WalkiesCookie.CookieName );
                var plainTextBytes = Encoding.UTF8.GetBytes( WalkiesCookie.Current.ToJsonString( new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" } ) );
                cookie.Value = Convert.ToBase64String( plainTextBytes );
                cookie.Expires = DateTime.Now.AddYears( 10 );
                HttpContext.Current.Response.SetCookie( cookie );
                HttpContext.Current.Items[ WalkiesCookie.CookieName ] = this;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted( ActionExecutedContext filterContext )
        {

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
