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
    public interface IDataEntityEvents<TModel, TEntity>
    {

        /* Occurs before an entity has its active state changed */
        bool OnBeforeChangeActiveState( TEntity entity, bool activeState );

        /* Occurs after an entity is first created in the database context */
        void OnAfterCreateEntity( TEntity entity );

        /* Occurs before a model is bound to the entity during a save operation */
        void OnBeforeBind( TModel model, TEntity entity );

        /* Occurs after a model is bound to the entity during a save operation */
        void OnAfterBind( TModel model, TEntity entity );

        /* Occurs before an entity is saved to the database context */
        void OnBeforeSaveEntity( TEntity entity );

        /* Occurs after an entity is saved to the database context */
        void OnAfterSaveEntity( TEntity entity, SaveState saveState, SaveContext saveContext );

    }

}
