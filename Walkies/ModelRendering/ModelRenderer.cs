using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Walkies.ViewRendering
{
    public class ModelRenderer
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private readonly ViewEngineCollection _viewEngines;
        private class StubController : Controller { }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// Creates a new <see cref="EmailViewRenderer"/> that uses the given view engines.
        /// </summary>
        /// <param name="_viewEngines">The view engines to use when rendering views.</param>
        /// <param name="viewDirectory">The directory under ~/Views that contains the views</param>
        public ModelRenderer( ViewEngineCollection viewEngines, string viewDirectory = "Emails" )
        {
            _viewEngines = viewEngines;
            this.ViewDirectory = viewDirectory;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Renders a view from a model
        /// </summary>
        /// <param name="email">The email to render.</param>
        /// <param name="viewName">Optional email view name override. If null then the email's ViewName property is used instead.</param>
        /// <returns>The rendered email view output.</returns>
        public string Render( ModelRenderBase model, string viewName = null )
        {
            viewName = viewName ?? model.ViewName;
            var controllerContext = CreateControllerContext( model.ViewDirectory );
            var view = this.CreateView( viewName, controllerContext );
            var viewOutput = this.RenderView( view, model.ViewData, controllerContext );
            return viewOutput;
        }

        /// <summary>
        /// Renders a view from a model and creates a 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public T Render<T>( ModelRenderBase<T> model, string viewName = null ) where T : IModelRenderResult, new()
        {
            viewName = viewName ?? model.ViewName;
            var controllerContext = CreateControllerContext( model.ViewDirectory );
            var view = this.CreateView( viewName, controllerContext );
            var viewOutput = this.RenderView( view, model.ViewData, controllerContext );

            var result = model.Result;
            result.Html = viewOutput;
            return result;

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
        /// 
        /// </summary>
        /// <returns></returns>
        private ControllerContext CreateControllerContext( string overrideViewDirectory )
        {

            // A dummy HttpContextBase that is enough to allow the view to be rendered.
            var httpContext = new HttpContextWrapper(
                new HttpContext(
                    new HttpRequest( "", UrlRoot(), "" ),
                    new HttpResponse( TextWriter.Null )
                )
            );

            var routeData = new RouteData();
            routeData.Values[ "controller" ] = string.IsNullOrEmpty( overrideViewDirectory ) ? this.ViewDirectory : overrideViewDirectory;
            var requestContext = new RequestContext( httpContext, routeData );
            var stubController = new StubController();
            var controllerContext = new ControllerContext( requestContext, stubController );
            stubController.ControllerContext = controllerContext;
            return controllerContext;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string UrlRoot()
        {

            var httpContext = HttpContext.Current;
            if( httpContext == null )
            {
                return "http://localhost";
            }

            return httpContext.Request.Url.GetLeftPart( UriPartial.Authority ) +
                   httpContext.Request.ApplicationPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        private IView CreateView( string viewName, ControllerContext controllerContext )
        {
            var result = _viewEngines.FindView( controllerContext, viewName, null );
            if( result.View != null )
            {
                return result.View;
            }
            throw new Exception( "View not found for " + viewName + ". Locations searched:" + Environment.NewLine + string.Join( Environment.NewLine, result.SearchedLocations ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="viewData"></param>
        /// <param name="controllerContext"></param>
        /// <param name="imageEmbedder"></param>
        /// <returns></returns>
        private string RenderView( IView view, ViewDataDictionary viewData, ControllerContext controllerContext )
        {
            using( var writer = new StringWriter() )
            {
                var viewContext = new ViewContext( controllerContext, view, viewData, new TempDataDictionary(), writer );
                view.Render( viewContext, writer );
                return writer.GetStringBuilder().ToString();
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public string ViewDirectory { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
