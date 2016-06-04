using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Walkies.Framework.Interfaces
{
    public interface IListedDataPageModel<TManager,TEntity>
    {

        TManager GetManager();
        Expression<Func<TEntity, bool>> OnCreateAuthorisationExpression();
        Expression<Func<TEntity, bool>> OnCreateFilterExpression();
        List<TEntity> Items { get; }

    }
}
