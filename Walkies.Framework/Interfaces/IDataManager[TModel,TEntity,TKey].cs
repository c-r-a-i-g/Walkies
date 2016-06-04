using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Walkies;
using Walkies.Framework.Web.DataTables;
using Walkies.Framework.Enumerations;
using Walkies.Core.Enumerations;

namespace Walkies.Framework.Interfaces
{
    public interface IDataManager<TModel, TEntity, TKey> : IDataEntityEvents<TModel, TEntity>
    {

        TEntity Find( TKey primaryKey );
        TEntity FindByExpression( string expression );
        TEntity SetActiveState( TKey key, bool isActive );
        IEnumerable<TEntity> GetList( Expression<Func<TEntity, bool>> expression = null, OrderByExpression order = null );
        FilteredList<TEntity> GetFilteredList( Expression<Func<TEntity, bool>> groupQuery = null, Expression<Func<TEntity, bool>> filterQuery = null, OrderByExpression order = null );
        SaveState Save();

        TModel Model { get; set; }

    }

}
