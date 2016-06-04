using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Walkies.Framework.BaseClasses;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Enumerations;
using Walkies.Framework.Web.DataTables;
using Walkies.Database;
using Walkies;
using Walkies.Database.Interfaces;
using Walkies.Core.Enumerations;
using Walkies.Framework.Caching;

namespace Walkies.Framework.BaseClasses
{
    public abstract class DataManagerBase<TManager, TModel, TEntity, TKey> : DataManagerContextBase<WalkiesDB, TManager, TModel, TEntity, TKey>
        where TModel : IDataPageModel<TManager, TEntity, TKey>
        where TEntity : class, new()
        where TManager : IDataManager<TModel, TEntity, TKey>, new()
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

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods
            
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties
            
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
