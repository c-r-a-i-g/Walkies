using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

using Microsoft.AspNet.Identity.Owin;

using Newtonsoft.Json;

using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Session;
using Walkies.Framework.Notifications;

using Walkies;
using Walkies.Mvc;
using Walkies.Framework.Enumerations;
using Walkies.Framework.Breadcrumbs;
using Walkies.Framework.PdfGenerator;
using Walkies.Framework.Web.Authorisation;
using Microsoft.Owin.Security;

namespace Walkies.Framework.BaseClasses
{
    public partial class WebControllerBase : Controller, IController
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private string _subFolder = "";

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        protected const string ApplicationJson = "application/json";

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Returns a view result that indicates that an error occurred and a save action was not performed
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult ErrorView( IPageModel model, string errorMessage )
        {
            model.Notify( errorMessage, NotificationType.Danger );
            return View( model );
        }

        /// <summary>
        /// Returns a view result that indicates that an error occurred and a save action was not performed
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult ErrorView( string viewName, IPageModel model, string errorMessage )
        {
            model.Notify( errorMessage, NotificationType.Danger );
            return View( viewName, model );
        }

        /// <summary>
        /// Returns a json result that indicates that a save action was performed , with a deliveredState indicated by
        /// the specified SaveState
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonSaveResult( SaveState state, string entityName, object data = null )
        {

            if( state.IsSuccessful )
            {
                return JsonSuccess( entityName, data );
            }

            if( state.IsAlreadyExists )
            {
                return JsonAlreadyExists( entityName, data );
            }

            return JsonError( entityName, data );

        }

        /// <summary>
        /// Sets a sub folder that can be used fluetly to show that the View or Partial should be searched
        /// for in the sub folder of the controllers folder
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        [NonAction]
        public WebControllerBase From( string subFolder )
        {
            _subFolder = subFolder;
            return this;
        }

        /// <summary>
        /// Returns a json result that indicates that a save action was performed , with a deliveredState indicated by
        /// the specified success flag
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonSaveResult( bool success, string entityName, object data = null )
        {

            if( success )
            {
                return JsonSuccess( entityName, data );
            }

            return JsonError( entityName, data );

        }

        /// <summary>
        /// Returns a json result that indicates that a save action was performed , with a deliveredState indicated by
        /// the specified success flag
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonSaveWithNotification( SaveState state, string message, object data = null )
        {

            var notifications = new List<Notification>();
            var notification = new Notification() { Content = message };
            notifications.Add( notification );

            var properties = new
            {
                notifications = notifications,
            };

            var json = this.Combine( data, properties );

            if( state.IsSuccessful )
            {
                json.success = true;
                notification.NotificationType = NotificationType.Success;
            }

            else
            {
                json.success = false;
                notification.NotificationType = NotificationType.Danger;
            }

            return Content( JsonConvert.SerializeObject( json ), ApplicationJson );

        }

        /// <summary>
        /// Returns a json result that indicates that a save action performed successfully
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonSuccess( string entityName, object data = null )
        {

            var properties = new
            {
                success = true,
                name = entityName
            };

            var json = this.Combine( data, properties );
            return Content( JsonConvert.SerializeObject( json ), ApplicationJson ); 

        }

        /// <summary>
        /// Returns a json result containing the specified notification
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonNotification( string message, NotificationType type, object data = null )
        {
            return JsonNotification( new Notification() { Content = message, NotificationType = type }, data );
        }

        /// <summary>
        /// Returns a json result containing the specified notification
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonNotification( Notification notification, object data = null )
        {

            var notifications = new List<Notification>();
            notifications.Add( notification );

            var properties = new
            {
                notifications = notifications,
            };

            var json = this.Combine( data, properties );
            return Content( JsonConvert.SerializeObject( json ), ApplicationJson );

        }

        /// <summary>
        /// Returns a json result that indicates that an error occurred and a save action was not performed
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonError( string entityName, object data = null )
        {

            var properties = new
            {
                success = false,
                name = entityName
            };

            var json = this.Combine( data, properties );
            return Content( JsonConvert.SerializeObject( json ), ApplicationJson ); 

        }

        /// <summary>
        /// Returns a json result that indicates that an newEntity already exists and a save action was not performed
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonAlreadyExists( string entityName, object data = null )
        {

            var properties = new
            {
                success = false,
                alreadyExists = true,
                name = entityName
            };

            var json = this.Combine( data, properties );
            return Content( JsonConvert.SerializeObject( json ), ApplicationJson ); 

        }

        /// <summary>
        /// Returns a result that takes the user to the specified action, with a notification that
        /// display the result of the specified SaveState
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectToRouteResult RedirectWithSaveNotification( string entityName, SaveState state, string actionName )
        {

            var notifications = new List<Notification>();
            
            if( state.IsSuccessful )
            {
                notifications.Add( new Notification()
                {
                    Content = string.Format( "The {0} has been saved.", entityName ),
                    NotificationType = NotificationType.Success
                } );
            }

            else if( state.IsError )
            {
                notifications.Add( new Notification()
                {
                    Content = string.Format( "Unfortunately there has been a problem.  The {0} has not been saved.", entityName ),
                    NotificationType = NotificationType.Danger
                } );
            }

            else if( state.IsAlreadyExists )
            {
                notifications.Add( new Notification()
                {
                    Content = string.Format( "There is already a matching record in the database.  The {0} has not been saved.", entityName ),
                    NotificationType = NotificationType.Warning
                } );
            }

            if( notifications.Count > 0 )
            {
                Session[ "Notifications" ] = notifications;
            }

            return RedirectToAction( actionName );

        }

        /// <summary>
        /// Returns a result that takes the user back to the previous page in the history, with a notification that
        /// display the result of the specified SaveState
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectResult BackWithSaveNotification( object entity, SaveState state )
        {

            var entityTitle = "";
            var entityTypeName = "record";

            if( entity != null )
            {
                var entityType = entity.GetType();
                entityTypeName = DataManager.NormaliseTypeName( entityType );
                entityTitle = entity.GetValueOfPropertyWithAttribute<EntityNameAttribute>( "" ).ToString();
            }

            return BackWithSaveNotification( entityTypeName, entityTitle, state );

        }

        /// <summary>
        /// Returns a result that takes the user back to the previous page in the history, with a notification that
        /// display the result of the specified SaveState
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectResult BackWithSaveNotification( string entityTypeName, string entityTitle, SaveState state )
        {

            var notifications = new List<Notification>();
            var namePhrase = string.IsNullOrEmpty( entityTitle ) ? "" : string.Format( "'{0}' ", entityTitle );

            if( state.IsSuccessful )
            {
                notifications.Add( new Notification()
                {
                    Content = string.Format( "The {0} {1}has been saved.", entityTypeName, namePhrase ),
                    NotificationType = NotificationType.Success
                } );
            }

            else if( state.IsError )
            {
                notifications.Add( new Notification()
                {
                    Content = string.Format( "Unfortunately there has been a problem.  The {0} has not been saved.", entityTypeName ),
                    NotificationType = NotificationType.Danger
                } );
            }

            else if( state.IsAlreadyExists )
            {
                notifications.Add( new Notification()
                {
                    Content = string.Format( "There is already a matching record in the database.  The {0} has not been saved.", entityTypeName ),
                    NotificationType = NotificationType.Warning
                } );
            }

            if( notifications.Count > 0 )
            {
                Session[ "notifications" ] = notifications;
            }

            return Redirect( BreadcrumbManager.BackUrl );

        }

        /// <summary>
        /// Returns a result that takes the user back to the previous page in the history, with a notification that
        /// display the result of the specified SaveState
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectResult BackWithNotification( string message, NotificationType notificationType )
        {
            var notifications = new List<Notification>();
            notifications.Add( new Notification() { Content = message, NotificationType = notificationType } );
            Session[ "notifications" ] = notifications;
            return Redirect( BreadcrumbManager.BackUrl );
        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public ActionResult ViewWithNotification( string message, NotificationType type )
        {
            return ViewWithNotification( null, null, message, type );
        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult ViewWithNotification( string viewName, string message, NotificationType type )
        {
            return ViewWithNotification( viewName, null, message, type );
        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult ViewWithNotification( object model, string message, NotificationType type )
        {
            return ViewWithNotification( null, model, message, type );
        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult ViewWithNotification( string viewName, object model, string message, NotificationType type )
        {

            var notifications = new List<Notification>();
            notifications.Add( new Notification { Content = message, NotificationType = type } );

            if( Request.IsAjaxRequest() )
            {
                return Json( new
                {
                    url = this.HttpContext.Request.Url.AbsoluteUri,
                    html = this.RenderToString( viewName, model ),
                    notifications = notifications
                }, JsonRequestBehavior.AllowGet );
            }

            else
            {
                if( notifications.Count > 0 )
                {
                    Session[ "notifications" ] = notifications;
                }
                return base.View( viewName, model );
            }

        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public new ActionResult View()
        {
            return View( null, null );
        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        [NonAction]
        public new ActionResult View( string viewName )
        {
            return View( viewName, null );
        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public new ActionResult View( object model )
        {
            return View( null, model );
        }

        /// <summary>
        /// Returns a view.  If the request is ajax, the view will be a json with the view contained in the html property.
        /// The json will also contain the url of the final request in case the result came from a redirect
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public new ActionResult View( string viewName, object model )
        {

            if( string.IsNullOrEmpty( viewName ) )
            {
                viewName = this.RouteData.GetRequiredString( "action" );
            }

            if( string.IsNullOrEmpty( _subFolder ) == false )
            {
                viewName = _subFolder + "/" + viewName;
            }

            if( Request.IsAjaxRequest() )
            {
                var url = Request.IsTransfer() ? Request.RewriteUrl() : this.HttpContext.Request.Url.AbsoluteUri;
                var html = this.RenderToString( viewName, model );
                return Json( new
                {
                    url = url,
                    success = ( string.IsNullOrEmpty( html ) == false ),
                    isModal = ( model is IModal ),
                    html = html
                }, JsonRequestBehavior.AllowGet );
            }

            else
            {
                return base.View( viewName, model );
            }

        }

        /// <summary>
        /// Returns a partial view.  If the request is ajax and the asJson flag is set (default), then 
        /// the result will be a json with the partial contained in the html property
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult PartialViewOrJson( string viewName, object model, bool asJson = true )
        {

            if( string.IsNullOrEmpty( viewName ) )
            {
                viewName = this.RouteData.GetRequiredString( "action" );
            }

            if( string.IsNullOrEmpty( _subFolder ) == false )
            {
                viewName = _subFolder + "/" + viewName;
            }

            if( Request.IsAjaxRequest() && asJson )
            {
                return Json( new
                {
                    html = this.RenderPartialToString( viewName, model )
                }, JsonRequestBehavior.AllowGet );
            }

            else
            {
                return base.PartialView( viewName, model );
            }

        }

        /// <summary>
        /// Returns a pdf view.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public ActionResult Pdf( Orientation orientation = Orientation.Portrait, string fileName = "" )
        {
            return Pdf( null, null, orientation, 0, fileName );
        }

        /// <summary>
        /// Returns a pdf view.
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult Pdf( string viewName, Orientation orientation = Orientation.Portrait, string fileName = "" )
        {
            return Pdf( viewName, null, orientation, 0, fileName );
        }

        /// <summary>
        /// Returns a pdf view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult Pdf( object model, Orientation orientation = Orientation.Portrait, string fileName = "" )
        {
            return Pdf( null, model, orientation, 0, fileName );
        }

        /// <summary>
        /// Returns a pdf view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult Pdf( string viewName, object model, Orientation orientation = Orientation.Portrait, string fileName = "" )
        {
            return Pdf( viewName, model, orientation, 0, fileName );
        }

        /// <summary>
        /// Returns a pdf view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult Pdf( object model, Orientation orientation, int javascriptDelay, string fileName = "" )
        {
            return Pdf( null, model, orientation, javascriptDelay, fileName );
        }

        /// <summary>
        /// Returns a pdf view.  Adds a header item that identifies the request as a pdf, which is later
        /// read by the ViewStart.cshtml, which renders the page with the PDF layout file.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="orientation"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult Pdf( string viewName, object model, Orientation orientation, int javascriptDelay, string fileName = "" )
        {

            // No view name was provided so use the name of the current action
            if( string.IsNullOrEmpty( viewName ) )
            {
                viewName = Request.RequestContext.RouteData.Values[ "action" ].ToString();
            }

            Request.Headers.Add( "pdf-request", "true" );
            Request.Headers.Add( "pdf-orientation", orientation == Orientation.Landscape ? "landscape" : "portrait" );
            var pdf = PdfGenerator.Pdf.FromView( this, viewName, model, orientation, javascriptDelay, fileName );

            if( pdf == null )
            {
                return Redirect( "/error/document-error" );
            }

            return pdf;

        }

        /// <summary>
        /// Transfers the result to the action found at the specified url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult TransferResult( string url )
        {
            var rewriteUrl = ControllerContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new TransferResult( url, rewriteUrl );
        }

        /// <summary>
        /// Transfers the result to the action found at the specified route and route values
        /// </summary>
        /// <param name="route"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult TransferResult( string route, RouteValueDictionary routeValues )
        {
            var rewriteUrl = ControllerContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new TransferToRouteResult( route, routeValues, rewriteUrl );
        }

        /// <summary>
        /// Transfers the result to the action found at the specified route values
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult TransferResult( RouteValueDictionary routeValues )
        {
            var rewriteUrl = ControllerContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new TransferToRouteResult( routeValues, rewriteUrl );
        }

        /// <summary>
        /// Transfers the result to the action found at the specified route values
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult TransferResult( string action, string controller )
        {
            var rewriteUrl = ControllerContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new TransferToRouteResult( new RouteValueDictionary( new { action = action, controller = controller } ), rewriteUrl );
        }

        /// <summary>
        /// Returns a plain JSON content response
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <param name="filteredCount"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult JsonContent( object data )
        {
            return Content( JsonConvert.SerializeObject( data ), ApplicationJson ); 
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Combine two generic object property containers into a single dynamic that is the result of the merge
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        private dynamic Combine( object item1, object item2 )
        {

            var dictionary1 = new RouteValueDictionary( item1 );
            var dictionary2 = new RouteValueDictionary( item2 );

            dynamic result = new ExpandoObject();
            var d = result as IDictionary<string, object>; 

            foreach( var pair in dictionary1 )
            {
                d[ pair.Key ] = pair.Value;
            }

            foreach( var pair in dictionary2 )
            {
                d[ pair.Key ] = pair.Value;
            }
            
            return result;

        }


        /// <summary>
        /// Renders a view to a string
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public string RenderToString()
        {
            return RenderToString( null, null );
        }

        /// <summary>
        /// Renders a view to a string
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        [NonAction]
        public string RenderToString( string viewName )
        {
            return RenderToString( viewName, null );
        }

        /// <summary>
        /// Renders a view to a string
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public string RenderToString( object model )
        {
            return RenderToString( null, model );
        }

        /// <summary>
        /// Renders a view to a string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public string RenderToString( string viewName, object model )
        {

            try
            {
                if( string.IsNullOrEmpty( viewName ) )
                {
                    viewName = ControllerContext.RouteData.GetRequiredString( "action" );
                }

                ViewData.Model = model;

                using( var stringWriter = new StringWriter() )
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindView( ControllerContext, viewName, "" );
                    ViewContext viewContext = new ViewContext( ControllerContext, viewResult.View, ViewData, TempData, stringWriter );
                    viewResult.View.Render( viewContext, stringWriter );
                    return stringWriter.GetStringBuilder().ToString();
                }
            }

            catch( Exception ex )
            {
                return "";
            }

        }

        /// <summary>
        /// Renders a partial view to a string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public string RenderPartialToString( string viewName, object model )
        {

            try
            {
                if( string.IsNullOrEmpty( viewName ) )
                {
                    viewName = ControllerContext.RouteData.GetRequiredString( "action" );
                }

                ViewData.Model = model;

                using( var stringWriter = new StringWriter() )
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView( ControllerContext, viewName );
                    ViewContext viewContext = new ViewContext( ControllerContext, viewResult.View, ViewData, TempData, stringWriter );
                    viewResult.View.Render( viewContext, stringWriter );
                    return stringWriter.GetStringBuilder().ToString();
                }
            }

            catch( Exception )
            {
                return "";
            }

        }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        /// <summary>
        /// Returns true if the request is being transferred
        /// </summary>
        public bool IsTransfer
        {
            get
            {
                return Request.Headers[ "is-transfer" ] == "true";
            }
        }

        /// <summary>
        /// If the request is being transferred, returns the rewrite url, i.e. the url that is to be displayed in the browser.
        /// </summary>
        public string RewriteUrl
        {
            get
            {
                return Request.Headers[ "rewrite-url" ];
            }
        }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Owin & Authentication

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }

}