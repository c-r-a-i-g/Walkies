using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Walkies.Framework.Breadcrumbs;
using Walkies.Framework.Cookies;
using Walkies.Framework.Enumerations;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Session;
using Walkies.Framework.Notifications;
using Walkies.Core.Enumerations;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.WebPages;

namespace Walkies.Framework.BaseClasses
{
    public class PageModelBase : IPageModel
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        internal static string DEFAULT_PAGE_TITLE = "NO PAGE TITLE SET";
        internal static string DEFAULT_SUB_TITLE = "NO SUB TITLE SET";

        private string _pageTitle = DEFAULT_PAGE_TITLE;
        private string _pageSubTitle = DEFAULT_SUB_TITLE;
        private RouteValueDictionary _pageData = new RouteValueDictionary();
        private WalkiesCookie _cookie = null;
        private string _encodedPageData = "";

        private PageHeaderType _pageHeaderType = PageHeaderType.Default;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public PageModelBase()
        {

            this.Notifications = new List<Notification>();
            this.Sidebar = Sidebar.None;
            this.Background = Background.Default;
            this.PageIcon = "glyphicon-paperclip";
            this.UpdateSidebarIfPageKeyChanges = false;
            this.IsFullScreen = false;
            this.IsUsingMainScrollbar = true;

            if( HttpContext.Current != null && HttpContext.Current.Request != null )
            {
                this.PageKey = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
            }

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Adds a notification to the page that will be displayed when the page loads
        /// </summary>
        /// <param name="text"></param>
        /// <param name="url"></param>
        /// <param name="class"></param>
        public void Notify( string content, NotificationType notificationType = NotificationType.Information )
        {
            this.Notifications.Add( new Notification
            {
                Content = content,
                NotificationType = notificationType
            } );
        }

        /// <summary>
        /// Gets the models page title to use as the breadcrumb title.  This is the default breadcrumb title, which
        /// can be overriden in the derived class if a specific page title is required to be generated from newEntity data
        /// </summary>
        /// <returns></returns>
        public virtual string GetBreadcrumbTitle()
        {
            return this.PageTitle;
        }

        /// <summary>
        /// Adds a piece of page data that will be encoded and sent to the client, where it will be unencoded and made
        /// available as a JavaScript object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddData( string key, object value )
        {
            _pageData.Add( key, value );
        }

        /// <summary>
        /// Maps the properties from the specified source object onto this model
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="ignoreProperties">An array of property names to ignore during the mapping</param>
        public void BindFrom<T>( T source, params string[] ignoreProperties )
        {
            AutoMap.Map( source, this, ignoreProperties );
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

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public List<Notification> Notifications { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public Sidebar Sidebar { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public Background Background { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public string PageIcon { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public string PageType { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public string PageKey { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public bool IsPinnable { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public bool UpdateSidebarIfPageKeyChanges { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public bool IsFullScreen { get; set; }

        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public bool IsUsingMainScrollbar { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets or sets the page title of the page.  Usually this will be called from the models view.  When set
        /// the BreadcrumbManager will be requested to update the breadcrumb of the current page, because when the
        /// breadcrumb was created we didnt have access to the page title.
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonProperty( "pageTitle" )]
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }

            set
            {
                _pageTitle = value;
                BreadcrumbManager.AddBreadcrumb( this );
            }
        }

        /// <summary>
        /// Gets or sets the page sub title of the newEntity.  
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonProperty( "pageSubTitle" )]
        public string PageSubTitle
        {
            get
            {
                return _pageSubTitle;
            }

            set
            {
                _pageSubTitle = value;
            }
        }

        /// <summary>
        /// Gets the breadcrumbs to be rendered.  The breadcrumbs are actually held on the users session as
        /// they must be persisted between pages.
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public List<Breadcrumb> Breadcrumbs
        {
            get
            {
                return WalkiesCookie.Current.Breadcrumbs;
            }
        }

        /// <summary>
        /// Gets the url that will take the user back one step in the breadcrumb trail or to the homepage if
        /// the breadcrumb trail is too short
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public string BackUrl
        {
            get
            {
                return BreadcrumbManager.BackUrl;
            }
        }

        /// <summary>
        /// Returns a flag that determines if the current page request is a from a page refresh.  Works in
        /// conjunction with RefreshFilter.cs
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public bool IsRefresh
        {
            get
            {
                var refreshFlag = HttpContext.Current.Items[ "IsRefreshed" ];
                return refreshFlag != null && (bool)refreshFlag == true;
            }
        }

        /// <summary>
        /// Returns an encoded string of json objects that are to be sent to the client
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public string EncodedPageData
        {
            get
            {

                if( string.IsNullOrEmpty( _encodedPageData ) )
                {

                    // Add the current user Id
                    _pageData.Add( "currentUserId", UserSession.Current.UserId );

                    // Add the data that can be used to create a bookmark from this page
                    if( this.IsPinnable )
                    {
                        _pageData.Add( "pinInfo", new
                        {
                            Title = this.PageTitle,
                            SubTitle1 = this.PageSubTitle,
                            SubTitle2 = "",
                            Key = this.PageKey
                        } );
                    }

                    // Look for items that were added to the HttpContext, i.e. from the UserSession
                    var items = HttpContext.Current.Items[ "page-data" ] as Dictionary<string, object>;
                    if( items != null )
                    {
                        foreach( var item in items )
                        {
                            _pageData.Add( item.Key, item.Value );
                        }
                    }

                    dynamic result = new ExpandoObject();
                    var d = result as IDictionary<string, object>;

                    foreach( var item in _pageData )
                    {
                        d[ item.Key ] = item.Value;
                    }

                    var bytes = Encoding.UTF8.GetBytes( _pageData.ToJsonString() );
                    _encodedPageData = Convert.ToBase64String( bytes );

                }

                return _encodedPageData;

            }
        }

        /// <summary>
        /// Returns an encoded string of json objects that are to be sent to the client
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public bool IsAjax
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.HttpContext.Request.IsAjaxRequest();
            }
        }

        /// <summary>
        /// Returns the current sales log cookie
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public WalkiesCookie Cookie
        {
            get
            {
                if( _cookie == null )
                {
                    _cookie = WalkiesCookie.Current;
                }
                return _cookie;
            }
        }

        /// <summary>
        /// Gets the controller that is controlling this page
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public string PageController
        {
            get
            {
                if( HttpContext.Current == null || HttpContext.Current.Request == null ) return "";
                return ( HttpContext.Current.Request.RequestContext.RouteData.Values[ "controller" ] as string ?? "Home" ).ToLower();
            }
        }

        /// <summary>
        /// Gets the action that is controlling this page
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public string PageAction
        {
            get
            {

                if( HttpContext.Current == null || HttpContext.Current.Request == null ) return "";
                return ( HttpContext.Current.Request.RequestContext.RouteData.Values[ "action" ] as string ?? "Home" ).ToLower();
            }
        }

        /// <summary>
        /// Gets the page header type
        /// </summary>
        [NotMapped]
        [AutoMapIgnore]
        [JsonIgnore]
        public PageHeaderType PageHeaderType
        {
            get
            {
                if ( UserSession.Current.UserId.HasValue == false )
                {
                    return PageHeaderType.AnonymousUser;
                }

                if ( UserSession.Current.RoleType == (int)RoleType.ApiUser )
                {
                    return PageHeaderType.WebApi;
                }

                return _pageHeaderType;
            }
            set
            {
                _pageHeaderType = value;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
