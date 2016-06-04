using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Walkies.Framework.Web.DataTables
{
    public class DataTablesResponseModel
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public DataTablesResponseModel( DataTablesRequestModel request, int totalCount, int filteredCount, List<object> data )
        {
            this.Draw = request.Draw;
            this.RecordsTotal = totalCount;
            this.RecordsFiltered = filteredCount;
            this.Data = data;
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

        /// <summary>
        /// Draw counter. This is used by DataTables to ensure that the Ajax returns from 
        /// server-side processing requests are drawn in sequence by DataTables 
        /// </summary>
        [JsonProperty( PropertyName = "draw" )]
        public int Draw { get; set; }

        /// <summary>
        /// The total number of records in the set, before any filtering is applied
        /// </summary>
        [JsonProperty( PropertyName = "recordsTotal" )]
        public int RecordsTotal { get; set; }

        /// <summary>
        /// The number of records filtered to if a term has been applied
        /// </summary>
        [JsonProperty( PropertyName = "recordsFiltered" )]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// The data to be rendered in the table
        /// </summary>
        [JsonProperty( PropertyName = "data" )]
        public List<object> Data { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
