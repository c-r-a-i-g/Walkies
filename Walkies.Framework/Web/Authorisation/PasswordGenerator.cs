using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Walkies.Core.Configuration;

namespace Walkies.Framework.Web.Authorisation
{
    public class PasswordGenerator
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
        /// Generates a random password using the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string Generate( PasswordGeneratorSettings settings )
        {

            string[] nouns = settings.Nouns.Split( ' ' );
            string[] verbs = settings.Verbs.Split( ' ' );
            string[] adjectives = settings.Adjectives.Split( ' ' );
            char[] digits = settings.Digits.ToCharArray();
            char[] symbols = settings.Symbols.ToCharArray();

            var random = new Random();
            var noun = nouns[ random.Next( 0, nouns.Length - 1 ) ];
            var verb = verbs[ random.Next( 0, verbs.Length - 1 ) ];
            var adjective = adjectives[ random.Next( 0, adjectives.Length - 1 ) ];
            var digit = digits[ random.Next( 0, digits.Length - 1 ) ];
            var symbol = symbols[ random.Next( 0, symbols.Length - 1 ) ];

            return verb + adjective + noun + digit + symbol;

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
