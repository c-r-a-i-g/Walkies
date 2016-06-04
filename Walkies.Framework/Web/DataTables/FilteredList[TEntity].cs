using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Walkies.Framework.Enumerations;
using Walkies.Core.Enumerations;

namespace Walkies.Framework.Web.DataTables
{
    public class FilteredList<TEntity>
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// Creates a filtered list with counts.
        /// </summary>
        /// <param name="collection">The collection to filter</param>
        /// <param name="authorisationQuery">A query that determines the entities that the user is authorised to access</param>
        /// <param name="filterQuery">A query that determines what entities are to be returned within the authorised collection</param>
        /// <param name="order">A query that determines the order of the results</param>
        public FilteredList( IQueryable<TEntity> collection, Expression<Func<TEntity, bool>> authorisationQuery = null, Expression<Func<TEntity, bool>> filterQuery = null, OrderByExpression order = null )
        {

            IQueryable<TEntity> entities = collection;

            if( authorisationQuery != null )
            {
                collection = collection.Where( authorisationQuery );
            }

            this.TotalCount = collection.Count();

            if( order == null )
            {
                this.Items = collection.Where( filterQuery ?? ( x => true ) );
            }

            else if( order.Direction == Direction.Ascending )
            {
                this.Items = Queryable.OrderBy( collection.Where( filterQuery ?? ( x => true ) ), order.Expression );
            }

            else 
            {
                this.Items = Queryable.OrderByDescending( collection.Where( filterQuery ?? ( x => true ) ), order.Expression );
            }

            this.FilteredCount = this.Items.Count();

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

        public int TotalCount { get; private set; }
        public int FilteredCount { get; private set; }
        public IEnumerable<TEntity> Items { get; private set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
