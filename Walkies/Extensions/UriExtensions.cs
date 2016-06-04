using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Walkies
{
    public static class UriExtensions
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private static readonly Regex _regex = new Regex( @"[?|&]([\w\.]+)=([^?|^&]+)" );

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Gets a dictionary from the querystring parameters
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, string> ParseQueryString( this Uri uri )
        {
            var match = _regex.Match( uri.PathAndQuery.ToLower() );
            var paramaters = new Dictionary<string, string>();
            while( match.Success )
            {
                paramaters.Add( match.Groups[ 1 ].Value, match.Groups[ 2 ].Value );
                match = match.NextMatch();
            }
            return paramaters;
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
