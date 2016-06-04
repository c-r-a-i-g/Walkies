using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Walkies.Core.Configuration;
using Walkies.Database;
using Walkies.Database.Entities;
using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Caching
{
    public class CacheBase<TManager,TEntity,TModel,TKey> where TEntity : class, new() where TManager : IDataManager<TModel, TEntity, TKey>, new()
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private object _padlock = new object();
        private static CacheBase<TManager, TEntity, TModel, TKey> _current = null;

        private ObjectCache _cache = null;
        private string _entityName = "";

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public CacheBase()
        {
            _entityName = typeof( TEntity ).Name;
            _cache = new MemoryCache( _entityName + "Cache" );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Adds an entity to the cache, where it will remain in memory as long as it is accessed within the cache time limit.
        /// If the entity is not retrieved within the cache time, it will be removed from memory.  If the entity is already
        /// in the cache, then it will be replaced with the updated version and the sliding expiration will be reset.
        /// </summary>
        /// <param name="entity"></param>
        public void Add( TEntity entity )
        {

            var key = this.GetPrimaryKey( entity );
            if( _cache.Contains( key ) )
            {
                this.Remove( key );
            }

            Debug.Print( "-> Entity has been added to the {0} cache: {1}", _entityName, key );

            lock( _padlock )
            {
                _cache.Add( key, entity, SlidingExpirationPolicy.New );
            }

        }

        /// <summary>
        /// Adds an entity to the cache, where it will remain in memory as long as it is accessed within the cache time limit.
        /// If the entity is not retrieved within the cache time, it will be removed from memory.  If the entity is already
        /// in the cache, then it will be replaced with the updated version and the sliding expiration will be reset.
        /// </summary>
        /// <param name="entity"></param>
        public void Add( TEntity entity, string expression )
        {
            
            if ( _cache.Contains( expression ) )
            {
                this.Remove( expression );
            }

            Debug.Print( "-> Entity has been added to the {0} cache: {1}", _entityName, expression );

            lock ( _padlock )
            {
                _cache.Add( expression, entity, SlidingExpirationPolicy.New );
            }

        }

        /// <summary>
        /// Removes the entity from the cache
        /// </summary>
        /// <param name="key"></param>
        public void Remove( string key )
        {
            Debug.Print( "-> Entity has been removed from the {0} cache: {1}", _entityName, key );
            lock( _padlock )
            {
                _cache.Remove( key );
            }
        }

        /// <summary>
        /// Retrieves a cached entity using the specified entityId.  If the entity does not exist in the
        /// cache, it will be retrieved from the database and added to the cache
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        public CacheFindResult<TEntity> Find( TKey entityId )
        {

            var key = entityId.ToString();

            if( _cache.Contains( key ) == false )
            {

                var manager = new TManager();
                var entity = manager.Find( entityId );

                if( entity != null )
                {
                    this.Add( entity );
                }

                return CacheFindResult<TEntity>.RetrievedFromDatabase( entity );

            }

            Debug.Print( "-> Entity has been retrieved from the {0} cache: {1}", _entityName, key );

            lock( _padlock )
            {
                return CacheFindResult<TEntity>.FoundInCache( _cache[ key ] as TEntity );
            }

        }

        /// <summary>
        /// Finds an entity based off an expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public CacheFindResult<TEntity> FindByExpression( string expression )
        {
            
            if ( _cache.Contains( expression ) == false )
            {
                TManager manager = new TManager();
                TEntity entity = manager.FindByExpression( expression );

                if ( entity != null )
                {
                    Add( entity, expression );
                }

                return CacheFindResult<TEntity>.RetrievedFromDatabase( entity );
            }

            Debug.Print( "-> Entity has been retrieved from the {0} cache: {1}", _entityName, expression );

            lock ( _padlock )
            {
                return CacheFindResult<TEntity>.FoundInCache( _cache[ expression ] as TEntity );
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Gets the primary key of the entity.  The entity must have a property that is decorated with KeyAttribute.
        /// </summary>
        /// <returns></returns>
        private string GetPrimaryKey( TEntity entity )
        {
            var primaryKey = entity.GetPropertyWithAttribute<KeyAttribute>();
            if( primaryKey == null )
            {
                throw new Exception( "Entity does not have a property that is decorated with the \"Key\" attribute.  You must decorate the primary key property of the entity with the \"Key\" attribute before attempting to save it." );
            }
            return primaryKey.GetValue( entity ).ToString();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets the underlying cache manager
        /// </summary>
        public static CacheBase<TManager,TEntity,TModel,TKey> Current
        {
            get
            {
                if( _current == null )
                {
                    _current = new CacheBase<TManager, TEntity, TModel, TKey>();
                }
                return _current;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
