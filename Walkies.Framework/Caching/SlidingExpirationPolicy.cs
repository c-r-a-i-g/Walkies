using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Walkies.Core.Configuration;
using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Caching
{
    public static class SlidingExpirationPolicy
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

        /// <summary>
        /// Callback that is fired before an item is removed from the cache.  Allows us to trigger events on
        /// the deal to flag that it was expired.
        /// </summary>
        /// <param name="arguments"></param>
        private static void CacheEntryRemovedHandler( CacheEntryRemovedArguments arguments )
        {

            var entityType = ObjectContext.GetObjectType( arguments.CacheItem.Value.GetType() ).Name;
            Debug.Print( "-> Cached entity was expired and has been removed from the {0} cache: {1}", entityType, arguments.CacheItem.Key );

            object expiredValue = arguments.CacheItem.Value;
            CacheEntryRemovedReason removalReason = arguments.RemovedReason;

            // Item has been removed from cache. Perform desired actions here, based on
            // the removal reason (for example, refresh the cache with the item).
            if( expiredValue != null && expiredValue is IDisposable )
            {

                if( expiredValue is ICacheExpiryHandler && removalReason == CacheEntryRemovedReason.Expired )
                {
                    ( (ICacheExpiryHandler)expiredValue ).CacheExpired();
                }

                ( (IDisposable)expiredValue ).Dispose();

            }

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets a sliding expiration time using settings configured in the settings
        /// </summary>
        public static CacheItemPolicy New
        {
            get
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromSeconds( ApplicationSettings.Current.DealCacheTimeout );
                policy.Priority = CacheItemPriority.Default;
                policy.RemovedCallback += new CacheEntryRemovedCallback( SlidingExpirationPolicy.CacheEntryRemovedHandler );
                return policy;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
