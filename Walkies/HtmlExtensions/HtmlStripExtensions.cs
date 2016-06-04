using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Walkies.Web
{
    public static class HtmlStripExtensions
    {
        /// <summary>
        /// Strips all html tags out of the provided string.
        /// </summary>
        public static MvcHtmlString StripHtmlTags( this HtmlHelper helper, string rawHtml )
        {
            return new MvcHtmlString( StripHtmlTagsToString( helper, rawHtml ) );
        }

        /// <summary>
        /// Strips all html tags out of the provided string.
        /// </summary>
        public static string StripHtmlTagsToString( this HtmlHelper helper, string rawHtml )
        {
            return Regex.Replace( rawHtml, "<(.|\\n)*?>", string.Empty );
        }

        /// <summary>
        /// Strips all html tags out of the provided string.
        /// </summary>
        public static MvcHtmlString StripHtmlTags( this HtmlHelper helper, MvcHtmlString rawHtml )
        {
            return new MvcHtmlString( StripHtmlTagsToString( helper, rawHtml.ToString() ) );
        }

        /// <summary>
        /// Strips all html tags out of the provided string.
        /// </summary>
        public static string StripHtmlTagsToString( this HtmlHelper helper, MvcHtmlString rawHtml )
        {
            return StripHtmlTagsToString( helper, rawHtml.ToString() );
        }
    }
}
