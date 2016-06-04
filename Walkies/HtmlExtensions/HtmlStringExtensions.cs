using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Walkies.Web
{
    public static class HtmlStringExtensions
    {
        /// <summary>
        /// Renders a partial to a string, for use in a view. Removes return and newline characters.
        /// </summary>
        public static MvcHtmlString RenderPartialToString( this HtmlHelper helper, string viewPath, object model )
        {
            return new MvcHtmlString( helper.Partial( viewPath, model ).ToString().Replace( "\r\n", string.Empty ) );
        }

        /// <summary>
        /// Escapes JSON single quotations.
        /// </summary>
        public static MvcHtmlString EscapeSingleQuotes( this MvcHtmlString htmlString )
        {
            return new MvcHtmlString( htmlString.ToString().Replace( "'", "\\'" ) );
        }
    }
}
