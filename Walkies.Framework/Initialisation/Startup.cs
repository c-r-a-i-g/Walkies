using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

using Walkies.Framework.Web.Authorisation;
using System.Web;

[assembly: OwinStartup( typeof( Walkies.Framework.Initialisation.Startup ) )]
namespace Walkies.Framework.Initialisation
{
    public partial class Startup
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members
            
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public void Configuration( IAppBuilder app )
        {

            Debug.Print( "AppBuilder Configuration Started" );

            var postStartupModuleType = typeof( IAppStartup );
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                 .SelectMany( s => s.GetLoadableTypes() )
                                 .Where( p => postStartupModuleType.IsAssignableFrom( p ) && p.IsAbstract == false );

            foreach( var type in types )
            {
                using( var instance = Activator.CreateInstance( type ) as IAppStartup )
                {
                    instance.Execute( app );
                }
            }
            
            Debug.Print( "AppBuilder Configuration Completed" );
            Debug.Print( "" );

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

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
