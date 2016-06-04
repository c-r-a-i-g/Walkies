using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Newtonsoft.Json;

using Walkies.Conversion;
using Walkies.Framework.BaseClasses;

namespace Walkies.Framework.Web.DataTables
{

    public class DataTablesRequestModel : PageModelBase
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public DataTablesRequestModel()
        {
            this.Search = new DataTablesSearch();
            this.Filters = new Dictionary<string, string>();
            this.Order = new List<DataTablesOrder>();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Retrieves a filter value from the filters collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public Nullable<T> GetFilterValue<T>( string name ) where T : struct
        {
            if( this.Filters.ContainsKey( name ) == false ) return null;
            if( this.Filters[ name ] == "" ) return null;
            return GenericConverter.ChangeType<T>( this.Filters[ name ] );
        }

        /// <summary>
        /// Adds a filter value to the filters collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetFilterValue( string name, object value )
        {
            if( this.Filters.ContainsKey( name ) )
            {
                this.Filters[ name ] = value.ToString();
                return;
            }

            if( value == null )
            {
                this.Filters.Add( name, "" );
            }

            else if( value.GetType().IsEnum )
            {
                this.Filters.Add( name, Convert.ToInt32( value ).ToString() );
            }

            else
            {
                this.Filters.Add( name, value.ToString() );
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

        /// <summary>
        /// Draw counter. This is used by DataTables to ensure that the Ajax returns from 
        /// server-side processing requests are drawn in sequence by DataTables 
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Paging first record indicator. This is the start point in the current data set 
        /// (0 index based - i.e. 0 is the first record)
        /// </summary>
        [JsonProperty( PropertyName = "start" )]
        public int Start { get; set; }

        public Dictionary<string, string> Filters { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw. It is expected
        /// that the number of records returned will be equal to this number, unless the 
        /// server has fewer records to return. Note that this can be -1 to indicate that 
        /// all records should be returned (although that negates any benefits of 
        /// server-side processing!)
        /// </summary>
        [JsonProperty( PropertyName = "length" )]
        public int Length { get; set; }

        /// <summary>
        /// Global Search for the table
        /// </summary>
        public DataTablesSearch Search { get; set; }

        /// <summary>
        /// Collection of all column indexes and their sort directions
        /// </summary>
        public IEnumerable<DataTablesOrder> Order { get; set; }

        /// <summary>
        /// Collection of all columns in the table
        /// </summary>
        public IEnumerable<DataTablesColumn> Columns { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties


        public bool HasSearch
        {
            get
            {
                return string.IsNullOrEmpty( this.Search.Value ) == false;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
