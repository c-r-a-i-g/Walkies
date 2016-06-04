using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.PdfGenerator
{
    public static class PdfFlags
    {

        public static string PrintMediaType = "--print-media-type ";
        public static string IgnoreLoadErrors = "--load-error-handling ignore ";
        public static string IgnoreMediaErrors = "--load-media-error-handling ignore ";
        public static string DebugJavascript = "--debug-javascript ";
        public static string Background = "--background ";
        public static string NoBackground = "--no-background ";
        public static string JavascriptDelay = "--enable-javascript --no-stop-slow-scripts --javascript-delay {0} ";

    }
}
