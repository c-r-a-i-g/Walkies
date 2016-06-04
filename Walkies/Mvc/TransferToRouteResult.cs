using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Walkies.Mvc
{
    public class TransferToRouteResult : ActionResult
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private string _rewriteUrl = "";

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public TransferToRouteResult( RouteValueDictionary routeValues, string rewriteUrl = "" ) : this( null, routeValues, rewriteUrl )
        {
        }

        public TransferToRouteResult( RouteValueDictionary routeValues ) : this( null, null, "" )
        {
        }

        public TransferToRouteResult( string routeName, RouteValueDictionary routeValues ) : this( routeName, routeValues, "" )
        {
        }

        public TransferToRouteResult( string routeName, RouteValueDictionary routeValues, string rewriteUrl = "" )
        {
            _rewriteUrl = rewriteUrl;
            this.RouteName = routeName ?? string.Empty;
            this.RouteValues = routeValues ?? new RouteValueDictionary();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public override void ExecuteResult( ControllerContext context )
        {

            if( context == null ) throw new ArgumentNullException( "context" );

            var urlHelper = new UrlHelper( context.RequestContext );
            var url = urlHelper.RouteUrl( this.RouteName, this.RouteValues );

            var actualResult = new TransferResult( url, _rewriteUrl );
            actualResult.ExecuteResult( context );

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

        public string RouteName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }

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
