using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

using Walkies.BootstrapHtml.Renderers;

namespace Walkies.BootstrapHtml
{
    public class Bootstrap
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public Bootstrap( ViewContext viewContext, IViewDataContainer viewDataContainer, HtmlHelper htmlHelper ) : this( viewContext, viewDataContainer, RouteTable.Routes, htmlHelper )
        {
        }

        public Bootstrap( ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection, HtmlHelper htmlHelper )
        {
            Html = htmlHelper;
            ViewContext = viewContext;
            ViewData = new ViewDataDictionary( viewDataContainer.ViewData );
        }

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
        /// Extracts the attributes into a collection
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        internal static RouteValueDictionary GetAttributes( object htmlAttributes = null, string @class = "" )
        {

            if( htmlAttributes == null )
            {
                htmlAttributes = new { };
            }

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes( htmlAttributes );
            attributes.Remove( "count" );
            attributes.Remove( "keys" );
            attributes.Remove( "values" );
            attributes.Remove( "length" );

            if( string.IsNullOrWhiteSpace( @class ) == false )
            {
                if( attributes.ContainsKey( "class" ) == false )
                {
                    attributes.Add( "class", @class );
                }
                else
                {
                    attributes[ "class" ] = attributes[ "class" ] + " " + @class;
                }
            }
            return attributes;

        }

        public static object GetModelStateValue( ViewDataDictionary viewData, string key, Type destinationType )
        {
            ModelState modelState;
            if( viewData.ModelState.TryGetValue( key, out modelState ) )
            {
                if( modelState.Value != null )
                {
                    return modelState.Value.ConvertTo( destinationType, null /* culture */ );
                }
            }
            return null;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public ViewDataDictionary ViewData { get; private set; }
        public ViewContext ViewContext { get; private set; }
        public HtmlHelper Html { get; private set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
