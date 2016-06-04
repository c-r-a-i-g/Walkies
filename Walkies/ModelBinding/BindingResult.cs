using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    /// <summary>
    /// Indicates the result of a custom binding.
    /// </summary>
    public sealed class BindingResult
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// Made private to force usage.
        /// </summary>
        private BindingResult()
        {

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Indicates that the binding succeeded.
        /// </summary>
        /// <returns></returns>
        public static BindingResult Success( object value )
        {
            return new BindingResult()
            {
                IsSuccessful = true,
                Value = value
            };
        }

        /// <summary>
        /// Indicates that the binding failed.
        /// </summary>
        public static BindingResult Failed( string errorMessage )
        {
            return new BindingResult()
            {
                IsSuccessful = false,
                ErrorMessage = errorMessage
            };
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        /// <summary>
        /// Gets whether or not the binding was a success.
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Gets the error message from the failed binding.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// The value to be bound.
        /// </summary>
        public object Value { get; private set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
