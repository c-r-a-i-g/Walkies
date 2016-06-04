using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Walkies
{
    public static class MemberInfoExtensions
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
        /// Gets the first attribute of the specified type from the object
        /// </summary>
        /// <returns></returns>
        public static T GetAttribute<T>( this MemberInfo member ) where T : Attribute
        {
            var attribute = member.GetCustomAttributes( typeof( T ), true ).FirstOrDefault();
            return attribute as T;
        }

        /// <summary>
        /// Gets all attributes of the specified type from the object
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributes<T>( this MemberInfo member ) where T : Attribute
        {
            var attribute = member.GetCustomAttributes( typeof( T ), true );
            return attribute as IEnumerable<T>;
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
