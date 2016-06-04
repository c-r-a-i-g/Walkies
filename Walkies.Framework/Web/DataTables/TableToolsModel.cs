using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Web.DataTables
{
    public class TableToolsModel
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public TableToolsModel() { }

        public TableToolsModel( IPageModel model, HelperResult options, HelperResult secondaryFilter = null, HelperResult additionalFilters = null )
        {
            this.Model = model;
            this.Options = options;
            this.SecondaryFilter = secondaryFilter;
            this.AdditionalFilters = additionalFilters;
        }

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

        public IPageModel Model { get; set; }
        public HelperResult Options { get; set; }
        public HelperResult SecondaryFilter { get; set; }
        public HelperResult AdditionalFilters { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public bool HasSecondaryFilter
        {
            get { return this.SecondaryFilter != null; }
        }

        public bool HasAdditionalFilters
        {
            get { return this.AdditionalFilters != null; }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
