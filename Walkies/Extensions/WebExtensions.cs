using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Walkies.Web
{
    public static class WebExtensions
    {
        /// <summary>
        /// Maps the given relative location to the absolute server location.
        /// </summary>
        /// <param name="location">The location to map absolutely.</param>
        /// <returns>The location absolutely mapped.</returns>
        public static string MapToServer( this string location )
        {
            if ( location.StartsWith( "~" ) )
            {
                return HttpContext.Current.Server.MapPath( location );
            }

            return HttpContext.Current.Server.MapPath( "~" + location );
        }
    }
}
