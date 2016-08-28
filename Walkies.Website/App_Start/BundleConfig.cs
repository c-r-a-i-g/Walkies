using System.Web;
using System.Web.Optimization;

namespace Walkies.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles( BundleCollection bundles )
        {
            bundles.Add( new ScriptBundle( "~/scripts/jquery" ).Include(
                        "~/Content/scripts/jquery/jquery-{version}.js",
                        "~/Content/scripts/jquery/jquery.validate*" ) );

            bundles.Add( new ScriptBundle( "~/scripts/bootstrap" ).Include(
                      "~/Content/scripts/modernizr/modernizr-*",
                      "~/Content/scripts/bootstrap/bootstrap.js",
                      "~/Content/scripts/bootstrap/respond.js" ) );

            bundles.Add( new StyleBundle( "~/css" ).Include(
                      "~/Content/css/bootstrap/bootstrap.min.css",
                      "~/Content/_themes/_default/theme.min.css" ) );

            bundles.Add( new ScriptBundle( "~/scripts/app" ).Include(
                        "~/Content/scripts/walkies.app.js" ) );
        }
    }
}
