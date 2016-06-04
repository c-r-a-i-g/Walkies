using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Web.DataTables
{
    public class DataTablesColumn
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

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

        /// <summary>
        /// Column's data source
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Column's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false)
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Flag to indicate if this column is searchable (true) or not (false)
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Search to apply to this specific column.
        /// </summary>
        public DataTablesSearch Search { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
