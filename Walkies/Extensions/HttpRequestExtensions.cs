using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Walkies
{
    public static class HttpRequestExtensions
    {

        /// <summary>
        /// Returns true if the request is flagged as a transfer
        /// </summary>
        /// <param name="context">The http request</param>
        /// <returns>True if the request has a transfer header</returns>
        public static bool IsTransfer( this HttpRequestBase request )
        {
            return request.Headers[ "is-transfer" ] == "true";
        }

        /// <summary>
        /// Returns the rewrite url from the header or null
        /// </summary>
        /// <param name="context">The http request</param>
        /// <returns>The rewrite url or null</returns>
        public static string RewriteUrl( this HttpRequestBase request )
        {
            return request.Headers[ "rewrite-url" ];
        }

        /// <summary>
        /// Returns true if the request is flagged as a transfer
        /// </summary>
        /// <param name="context">The http request</param>
        /// <returns>True if the request has a transfer header</returns>
        public static bool IsTransfer( this HttpRequest request )
        {
            return request.Headers[ "is-transfer" ] == "true";
        }

        /// <summary>
        /// Returns the rewrite url from the header or null
        /// </summary>
        /// <param name="context">The http request</param>
        /// <returns>The rewrite url or null</returns>
        public static string RewriteUrl( this HttpRequest request )
        {
            return request.Headers[ "rewrite-url" ];
        }

    }

}
