using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Walkies
{

    public static class AppDomainExtensions
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Gets the base directory in which the application is running.  This will be the bin directory
        /// </summary>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public static string GetBaseDirectory( this AppDomain appDomain )
        {
            if( HttpContext.Current == null ) return appDomain.BaseDirectory;
            else return Path.Combine( appDomain.BaseDirectory, "bin" );
        }

        /// <summary>
        /// Gets the root directory of the running application.  This is the root folder of the project
        /// that is executing
        /// </summary>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public static string GetRootDirectory( this AppDomain appDomain )
        {

            string baseDirectory = "";

            if( HttpContext.Current == null ) baseDirectory = appDomain.BaseDirectory;
            else baseDirectory = Path.Combine( appDomain.BaseDirectory, "bin" );

            baseDirectory = baseDirectory.Replace( "/", @"\\" ).ToLower();
            baseDirectory = baseDirectory.TrimEnd( '\\' );

            while( baseDirectory.EndsWith( @"\bin" ) || baseDirectory.EndsWith( @"\debug" ) )
            {
                baseDirectory = Directory.GetParent( baseDirectory ).FullName;
            }

            return baseDirectory;

        }

        /// <summary>
        /// Gets the content directory of the root directory of the running application.  i.e. from the root folder of the project
        /// that is executing
        /// </summary>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public static string GetContentDirectory( this AppDomain appDomain )
        {
            string baseDirectory = appDomain.GetRootDirectory(); ;
            return Path.Combine( baseDirectory, "Content" );
        }

        /// <summary>
        /// Gets the settings directory of the root directory of the running application.  i.e. from the root folder of the project
        /// that is executing
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="relativePathToSettingsFolder"></param>
        /// <returns></returns>
        public static string GetSettingsDirectory( this AppDomain appDomain, string relativePathToSettingsFolder = "_settings" )
        {
            string baseDirectory = appDomain.GetRootDirectory(); ;
            return Path.Combine( baseDirectory, relativePathToSettingsFolder );
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
