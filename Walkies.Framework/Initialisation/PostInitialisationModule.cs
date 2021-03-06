﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using WebActivatorEx;

using Walkies.Framework;
using Walkies.Framework.Initialisation;
using System.Threading;
using Microsoft.Owin;

[assembly: PostApplicationStartMethod( typeof( PostInitialisationModule ), "PostInitialisation" )]
public class PostInitialisationModule
{

    /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    #region Class Members

    #endregion

    /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    #region Constructor & Intialisation

    #endregion

    /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    #region Public Methods

    public static void PostInitialisation()
    {

        Debug.Print( "Post Initialisation" );

        var postStartupModuleType = typeof( IPostStartup );
        var types = AppDomain.CurrentDomain.GetAssemblies()
                             .SelectMany( s => s.GetLoadableTypes() )
                             .Where( p => postStartupModuleType.IsAssignableFrom( p ) && p.IsAbstract == false );

        foreach( var type in types )
        {
            using( var instance = Activator.CreateInstance( type ) as IPostStartup )
            {
                instance.Execute( HttpContext.Current.ApplicationInstance );
            }
        }

        Debug.Print( "Post Initialisation Complete" );
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

