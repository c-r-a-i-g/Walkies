using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Walkies
{
    public static class AuthorizationContextExtensions
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
        /// Gets a data item from the route data using a case insensitive key
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static dynamic GetRouteData<T>( this AuthorizationContext context, string key )
        {

            try
            {
                var routeData = context.RequestContext.RouteData;
                var normalisedKey = routeData.Values.Keys.FirstOrDefault( k => k.ToLower() == key.ToLower() );
                if( string.IsNullOrEmpty( normalisedKey ) ) return null;

                if( typeof( T ) == typeof( Guid ) )
                {
                    return Guid.Parse( routeData.Values[ normalisedKey ].ToString()  );
                }

                else if( typeof( T ) == typeof( int ) )
                {
                    return int.Parse( routeData.Values[ normalisedKey ].ToString() );
                }

                else if( typeof( T ) == typeof( decimal ) )
                {
                    return decimal.Parse( routeData.Values[ normalisedKey ].ToString() );
                }

                else if( typeof( T ) == typeof( double ) )
                {
                    return double.Parse( routeData.Values[ normalisedKey ].ToString() );
                }

                else if( typeof( T ) == typeof( bool ) )
                {
                    return bool.Parse( routeData.Values[ normalisedKey ].ToString() );
                }

                else if( typeof( T ) == typeof( float ) )
                {
                    return float.Parse( routeData.Values[ normalisedKey ].ToString() );
                }

                else if( typeof( T ) == typeof( byte ) )
                {
                    return byte.Parse( routeData.Values[ normalisedKey ].ToString() );
                }

                return routeData.Values[ normalisedKey ].ToString();
            }

            catch( Exception )
            {
                return null;
            }

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
