using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Walkies.Mvc
{
    public class TransferResult : ActionResult
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private string _rewriteUrl = "";

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public TransferResult( string url, string rewriteUrl = "" )
        {
            _rewriteUrl = rewriteUrl;
            this.Url = url;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Executes the transfer
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult( ControllerContext context )
        {

            if( context == null ) throw new ArgumentNullException( "context" );

            var httpContext = HttpContext.Current;

            httpContext.Request.Headers.Add( "is-transfer", "true" );
            httpContext.Request.Headers.Add( "rewrite-url", _rewriteUrl );

            // MVC 3 running on IIS 7+
            if( HttpRuntime.UsingIntegratedPipeline )
            {
                httpContext.Server.TransferRequest( this.Url, true );
            }

            else
            {
                // Pre MVC 3
                httpContext.RewritePath( this.Url, false );

                IHttpHandler httpHandler = new MvcHttpHandler();
                httpHandler.ProcessRequest( httpContext );
            }

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

        public string Url { get; private set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
