using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

using Newtonsoft.Json;

using Walkies.Framework.Cookies;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Authorisation;
using Walkies.Database;
using Walkies.Database.Entities;
using Walkies.Core.Enumerations;
using System.Diagnostics;

namespace Walkies.Framework.Web.Session
{
    public class UserSession 
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        public static string SESSION_KEY = "user-session";
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public UserSession()
        {
        }

        public UserSession( HttpContext context, string username = "" ) : this()
        {

            if( string.IsNullOrEmpty( username ) == false )
            {
                this.InitialiseForAuthenticatedUser( context, username );
            }

            else if( string.IsNullOrEmpty( context.User.Identity.Name ) == false )
            {
                this.InitialiseForAuthenticatedUser( context, context.User.Identity.Name );
            }

            else
            {
                this.InitialiseForAnonymousUser( context );
            }

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Returns true if the user has the specified permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool HasPermission( params Permission[] permissions )
        {
            return this.Secure.UserPermissions.Count( p => permissions.Contains( p ) ) > 0;
        }

        /// <summary>
        /// Gets the current domain settings
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns></returns>
        public User GetUser()
        {
            if( this.UserId.HasValue == false ) return null;
            var db = new WalkiesDB();
            return db.Users.Find( this.UserId );
        }

        /// <summary>
        /// Adds a piece of page data that will be encoded and sent to the client, where it will be unencoded and made
        /// available as a JavaScript object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddData( string key, object value )
        {

            var items = HttpContext.Current.Items[ "page-data" ] as Dictionary<string,object>;
            if( items == null )
            {
                items = new Dictionary<string, object>();
                HttpContext.Current.Items.Add( "page-data", items );
            }
            
            if( items.Keys.Contains( key ) == false )
            {
                items.Add( key, value );
            }

        }

        /// <summary>
        /// Changes the theme to the specified theme.
        /// </summary>
        /// <param name="theme">The new theme to use</param>
        public void ChangeTheme( string theme )
        {
            Theme = "_default";
            WalkiesCookie.Current.Theme = theme;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        /// <summary>
        /// Initialises the user session for an anonymous user
        /// </summary>
        /// <param name="context"></param>
        protected virtual void InitialiseForAnonymousUser( HttpContext context )
        {

            var db = new WalkiesDB();
            var domain = context.Request.Url.Authority;

            this.ApplicationName = "Walkies";
            this.Theme = "_default";
            this.Domain = domain.ToLower();
            this.AllowThemeChange = false;
            this.SessionId = Guid.NewGuid();
            this.Secure.IsAuthenticated = false;
            this.Secure.UserPermissions = new List<Permission>();
            this.Secure.RoleType = 0;

        }

        /// <summary>
        /// Initialises the user session for an authenticated user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        protected void InitialiseForAuthenticatedUser( HttpContext context, string username )
        {

            var db = new WalkiesDB();
            var user = db.Users.FirstOrDefault( x => x.UserName == username );
            var domain = context.Request.Url.Authority;

            if( user == null ) return;

            this.AllowThemeChange = false;
            this.SessionId = Guid.NewGuid();
            this.UserId = user.UserId;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Domain = domain;
            this.ApplicationName = "Walkies";
            this.HasAgreedToTerms = user.DateAgreedTerms.HasValue; // TODO: place a check on the date of the current terms
            this.Secure.IsAuthenticated = true;

            ChangeTheme( "_default" );

        }

        /// <summary>
        /// Occurs once per page request, the first time the user session is accessed
        /// </summary>
        protected void OnRequest()
        {

            Debug.Print( "-- UserSession.OnRequest()" );

            // Add the users pinned items to the context so that they can be serialised and returned to the client
            var wrapper = new HttpRequestWrapper( HttpContext.Current.Request );
            var isAjax = wrapper.IsAjaxRequest();
            var isLogin = HttpContext.Current.Session[ "OnLogIn" ] != null;

            if( isLogin || ( this.IsAuthenticated && isAjax == false ) )
            {
                var db = new WalkiesDB();
                HttpContext.Current.Session.Remove( "OnLogIn" );
            }

            if( isLogin )
            {
                this.OnLogin();
            }

        }

        /// <summary>
        /// Occurs on the request following the login redirection.
        /// </summary>
        protected void OnLogin()
        {
            Debug.Print( "-- UserSession.OnAfterLogin()" );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// Refreshes the user session.  Called by the RefreshFilter.cs on the page request if the page
        /// requested is the result of a refresh action, but can be called at any time if a significant
        /// change is made to the user that needs to be reflected.
        /// </summary>
        public static void Refresh()
        {
            var cookie = WalkiesCookie.Current;
            cookie.UserSession = new UserSession( HttpContext.Current );
            cookie.IsRefresh = true;
            cookie.Save();
        }

        /// <summary>
        /// Destroys the user session following a logoff event
        /// </summary>
        public static void Logoff()
        {
            WalkiesCookie.ClearUserSession();
        }

        /// <summary>
        /// Initialises the user session following a login event
        /// </summary>
        /// <param name="username"></param>
        public static void Login( string username )
        {
            
            var cookie = WalkiesCookie.Current;
            cookie.UserSession = new UserSession( HttpContext.Current, username.Trim().ToLower() );
            cookie.Save();
        }

        /// <summary>
        /// Returns true if the current user has the specified permission
        /// </summary>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public static bool Can( Permission permission )
        {
            return UserSession.Current.Permissions.Exists( x => x == permission || x == Permission.Administrator );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public Guid? UserId { get; set; }
        public Guid ClientId { get; set; }
        public Guid? DomainSettingId { get; set; }
        public Guid SessionId { get; set; }
        public string ApplicationName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Theme { get; set; }
        public string Domain { get; set; }
        public bool HasAgreedToTerms { get; set; }
        public bool AllowThemeChange { get; set; }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets the current user session data
        /// </summary>
        public static UserSession Current
        {
            get
            {

                var cookie = WalkiesCookie.Current;
                var userSession = cookie.UserSession;
                var isUserSessionRecreated = false;

                if( HttpContext.Current.Request.Path.Contains( "signalr" ) == false )
                {

                    if( HttpContext.Current.Request.IsAuthenticated == false && cookie.UserSession.IsAuthenticated )
                    {
                        cookie.UserSession = new UserSession( HttpContext.Current );
                        cookie.Save();
                        isUserSessionRecreated = true;
                    }

                    else if( HttpContext.Current.Request.IsAuthenticated && ( cookie.UserSession.UserId.HasValue == false || cookie.UserSession.IsAuthenticated == false ) )
                    {
                        cookie.UserSession = new UserSession( HttpContext.Current, HttpContext.Current.User.Identity.Name );
                        cookie.Save();
                        isUserSessionRecreated = true;
                    }

                    else if( ( userSession == null || userSession.IsInitialised == false ) && userSession.IsAuthenticated == false )
                    {
                        cookie.UserSession = new UserSession( HttpContext.Current );
                        cookie.Save();
                        isUserSessionRecreated = true;
                    }

                    if( isUserSessionRecreated )
                    {
                        HttpContext.Current.Items[ "isUserSessionRecreated" ] = true;
                    }

                    if( cookie.UserSession.ShouldRaiseOnRequestEvent )
                    {
                        cookie.UserSession.OnRequest();
                    }

                }

                return cookie.UserSession;

            }
        }

        /// <summary>
        /// Gets the full name of the user
        /// </summary>
        [JsonIgnore]
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Secure Property Accessors

        /// <summary>
        /// Gets the secure data structure from the session state.  If it doesnt exist it will be
        /// created and then saved into the session.  The secure data object should contain any user session
        /// data that should not be sent in the cookie down to the client
        /// </summary>
        [JsonIgnore]
        public SecureData Secure
        {
            get
            {

                if( HttpContext.Current.Session != null )
                {
                    var secure = HttpContext.Current.Session[ SESSION_KEY ] as SecureData;
                    if( secure == null )
                    {
                        secure = new SecureData();
                        HttpContext.Current.Session[ SESSION_KEY ] = secure;
                    }
                    return secure;
                }

                return new SecureData();

            }
        }

        /// <summary>
        /// Gets or sets a flag that denotes if the OnRequest event has been triggered
        /// </summary>
        private bool ShouldRaiseOnRequestEvent
        {

            get
            {
                if( HttpContext.Current.Items[ "HasCheckedForOnRequestEvent" ] != null )
                {
                    return false;
                }

                HttpContext.Current.Items[ "HasCheckedForOnRequestEvent" ] = true;

                var wrapper = new HttpRequestWrapper( HttpContext.Current.Request );
                var isAjax = wrapper.IsAjaxRequest();
                if( isAjax == false || HttpContext.Current.Request.Headers.AllKeys.Contains( "X-Walkies-PAGE" ) )
                {
                    return true;
                }
                return false;

            }

            set
            {
                HttpContext.Current.Items[ "isOnRequestTriggered" ] = value;
            }

        }
        /// <summary>
        /// Returns true if the user session is in an initialised state
        /// </summary>
        [JsonIgnore]
        public bool IsInitialised
        {
            get { return string.IsNullOrEmpty( this.Theme ) == false && string.IsNullOrEmpty( this.Domain ) == false; }
        }

        /// <summary>
        /// Gets a flag that denotes if the user is authenticated
        /// </summary>
        [JsonIgnore]
        public bool IsAuthenticated
        {
            get { return this.Secure != null && this.Secure.IsAuthenticated; }
        }

        /// <summary>
        /// Returns true if the user has the administrator permission
        /// </summary>
        [JsonIgnore]
        public bool IsAdministrator
        {
            get { return this.Permissions.Exists( x => x == Permission.Administrator ); }
        }

        /// <summary>
        /// Gets the users role type
        /// </summary>
        [JsonIgnore]
        public int RoleType
        {
            get { return this.Secure.RoleType; }
        }

        /// <summary>
        /// Gets the users permissions
        /// </summary>
        [JsonIgnore]
        public List<Permission> Permissions
        {
            get { return this.Secure.UserPermissions; }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
