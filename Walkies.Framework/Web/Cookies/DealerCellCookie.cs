using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Walkies.Framework.Breadcrumbs;
using Walkies.Framework.Web.Session;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Walkies.Framework.Cookies
{
    public class WalkiesCookie
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private static string _cookieName = "ChocolateChip";

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public WalkiesCookie() { }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Serialises the cookie and stores it in the response.
        /// </summary>
        public void Save()
        {
            var cookie = new HttpCookie( _cookieName );
            var plainTextBytes = Encoding.UTF8.GetBytes( this.ToJsonString( new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" } ) );
            cookie.Value = Convert.ToBase64String( plainTextBytes );
            cookie.Expires = DateTime.Now.AddYears( 10 );
            HttpContext.Current.Response.SetCookie( cookie );
            HttpContext.Current.Items[ _cookieName ] = this;
            HttpContext.Current.Request.Cookies.Set( cookie );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// Gets the current Cookie or creates a new one if it doesnt exist
        /// </summary>
        public static WalkiesCookie Current
        {
            get
            {

                WalkiesCookie cookie = null;

                try
                {
                    // The cookie has been retrieved already in this request, so grab it
                    if ( HttpContext.Current.Items.Contains( _cookieName ) )
                    {
                        return HttpContext.Current.Items[ _cookieName ] as WalkiesCookie;
                    }

                    // The cookie is already in the request context so retrieve it
                    else if ( HttpContext.Current.Request.Cookies[ _cookieName ] != null )
                    {
                        var cookieContainer = HttpContext.Current.Request.Cookies[ _cookieName ];
                        var encodedBytes = Convert.FromBase64String( HttpUtility.UrlDecode( cookieContainer.Value ) );
                        var json = Encoding.UTF8.GetString( encodedBytes );
                        cookie = JsonConvert.DeserializeObject<WalkiesCookie>( json );
                    }

                    // The cookie doesn't exist in the request context yet, so create a new one
                    // and add it
                    if ( cookie == null )
                    {
                        cookie = new WalkiesCookie();
                        cookie.UserSession = new UserSession();
                        cookie.Breadcrumbs = new List<Breadcrumb>();
                        cookie.Save();
                    }

                    HttpContext.Current.Items[ _cookieName ] = cookie;
                    return cookie;
                }
                catch( Exception )
                {
                    return new WalkiesCookie();
                }

            }
        }

        /// <summary>
        /// Clears the user session in the cookie, e.g. when a user logs off
        /// </summary>
        public static void ClearUserSession()
        {
            var cookie = WalkiesCookie.Current;
            cookie.UserSession = new UserSession(); 
            cookie.Save();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public UserSession UserSession { get; set; }
        public List<Breadcrumb> Breadcrumbs { get; set; }
        public bool IsTransfer { get; set; }
        public string RewriteUrl { get; set; }
        public string RefreshUrl { get; set; }
        public bool IsRefresh { get; set; }
        public bool DisplayInactive { get; set; }
        public string Theme { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public static string CookieName
        {
            get { return _cookieName; }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
