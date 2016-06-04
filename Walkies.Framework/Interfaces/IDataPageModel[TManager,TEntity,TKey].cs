using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Walkies.Framework.Interfaces
{
    public interface IDataPageModel<TManager, TEntity, TKey>
    {
        void Bind( TKey primaryKey );
        void Bind( TEntity entity );
        void BindTo( TEntity entity );
        void BindFrom<T>( T entity, params string[] ignoreProperties );
        void OnBeforeBind( TEntity entity );
        void OnAfterBind( TEntity entity );
        void OnModelSaved( SaveState saveState );
        SaveState Save();
        TEntity Entity { get; }

    }
}
