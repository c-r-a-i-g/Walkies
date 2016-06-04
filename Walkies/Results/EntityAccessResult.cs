using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public class EntityAccessResult
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        /// <summary>
        /// Holds whether or not access was granted to the desired entity.
        /// </summary>
        private bool _isGranted;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// Made private to promote use of static creators.
        /// </summary>
        private EntityAccessResult()
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
        /// Creates an EntityAccessResult that states access was granted.
        /// </summary>
        public static EntityAccessResult Granted()
        {
            return ( new EntityAccessResult()
                {
                    _isGranted = true
                } );
        }

        /// <summary>
        /// Creates an EntityAccessResult that states access was denied.
        /// </summary>
        public static EntityAccessResult Denied( string reason )
        {
            return ( new EntityAccessResult()
                {
                    _isGranted = false,
                    Reason = reason
                } );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        /// <summary>
        /// Returns whether or not access was granted to desired entity.
        /// </summary>
        public bool IsGranted { get { return ( this._isGranted == true ); } }

        /// <summary>
        /// Returns whether or not access was denied to desired entity.
        /// </summary>
        public bool IsDenied { get { return ( this._isGranted == false ); } }

        /// <summary>
        /// Returns the reason (if any) why access was denied.
        /// </summary>
        public string Reason { get; private set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
