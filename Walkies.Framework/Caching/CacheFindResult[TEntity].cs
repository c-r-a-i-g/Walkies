using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Core.Enumerations;
using Walkies.Database.Entities;

namespace Walkies.Framework.Caching
{
    public class CacheFindResult<TEntity> where TEntity : class
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

        public static CacheFindResult<TEntity> FoundInCache( TEntity entity )
        {
            return new CacheFindResult<TEntity>
            {
                Entity = entity,
                State = CacheFindState.FoundInCache
            };
        }

        public static CacheFindResult<TEntity> NotFoundInCache()
        {
            return new CacheFindResult<TEntity>
            {
                Entity = null,
                State = CacheFindState.NotFoundInCache
            };
        }

        public static CacheFindResult<TEntity> RetrievedFromDatabase( TEntity entity )
        {
            return new CacheFindResult<TEntity>
            {
                Entity = entity,
                State = CacheFindState.RetrievedFromDatabase
            };
        }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public TEntity Entity { get; set; }
        public CacheFindState State { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
