using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
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
    public abstract class DataManagerContextBase<TContext, TManager, TModel, TEntity, TKey> : IAuthorisor, IDataManager<TModel, TEntity, TKey>
        where TContext : DbContext, new()
        where TModel : IDataPageModel<TManager, TEntity, TKey>
        where TEntity : class, new()
        where TManager : IDataManager<TModel, TEntity, TKey>, new()
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        protected TContext _db { get; private set; }
        private static CacheBase<TManager, TEntity, TModel, TKey> _cache = null;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public DataManagerContextBase()
        {
            _db = new TContext();
        }

        public DataManagerContextBase( TModel model ) : this()
        {
            this.Model = model;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public virtual SaveState Save()
        {

            var primaryKeyValue = this.GetModelPrimaryKeyValue();
            if( primaryKeyValue == null )
            {
                return this.CreateEntity();
            }

            return this.UpdateEntity();

        }

        /// <summary>
        /// Retrieves the entities to be listed
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetList( Expression<Func<TEntity, bool>> expression = null, OrderByExpression order = null )
        {

            var collection = this.GetCollection();

            if( expression == null && order == null )
            {
                return collection.ToList();
            }

            else if( expression == null && order.Direction == Direction.Ascending )
            {

                return Queryable.OrderBy( collection, order.Expression ).ToList();
            }

            if( expression == null && order.Direction == Direction.Descending )
            {
                return Queryable.OrderByDescending( collection, order.Expression ).ToList();
            }
            
            if( order == null )
            {
                return collection.Where( expression );
            }

            else if( order.Direction == Direction.Ascending )
            {
                return Queryable.OrderBy( collection.Where( expression ), order.Expression );
            }

            else
            {
                return Queryable.OrderByDescending( collection.Where( expression ), order.Expression );
            }

        }

        /// <summary>
        /// Retrieves the entities to be listed
        /// </summary>
        /// <returns></returns>
        public virtual FilteredList<TEntity> GetFilteredList( Expression<Func<TEntity, bool>> groupQuery = null, Expression<Func<TEntity, bool>> filterQuery = null, OrderByExpression order = null )
        {
            var collection = this.GetCollection();
            return new FilteredList<TEntity>( collection, groupQuery, filterQuery, order );
        }

        /// <summary>
        /// HasPriviledge be overriden in the derived class to determine if the current user is allowed to access a 
        /// database newEntity depending on their permissions and position within the client structure. Implemented
        /// through the IAuthorise attribute, this function can be implemented in any class
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool CanAccess( AuthorizationContext context )
        {
            return true;
        }

        /// <summary>
        /// Finds the newEntity with the specified primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public virtual TEntity Find( TKey primaryKey )
        {
            return this.FindWithPrimaryKey( primaryKey );
        }

        /// <summary>
        /// Finds the entity using its name.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual TEntity FindByExpression( string expression )
        {
            return this.GetCollection().Where( expression ).FirstOrDefault();
        }

        /// <summary>
        /// Binds the managed newEntity onto the specified newEntity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="newEntity"></param>
        public void Bind( TEntity entity )
        {
            this.Model.OnBeforeBind( entity );
            this.OnBeforeBind( this.Model, entity );
            this.Model.BindTo( entity );
            this.Model.OnAfterBind( entity );
            this.OnAfterBind( this.Model, entity );
        }

        /// <summary>
        /// Creates a new instance of TEntity, binds the managed newEntity onto it and returns the result
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="newEntity"></param>
        public TEntity NewEntity()
        {
            var entity = new TEntity();
            this.Model.OnBeforeBind( entity );
            this.OnBeforeBind( this.Model, entity );
            this.Model.BindTo( entity );
            this.Model.OnAfterBind( entity );
            this.OnAfterBind( this.Model, entity );
            return entity;
        }

        /// <summary>
        /// Sets the active deliveredState of the record identified by the specified key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="activeState"></param>
        public virtual TEntity SetActiveState( TKey primaryKey, bool activeState )
        {
            var entity = this.GetCollection().Find( primaryKey );
            if( entity == null ) return null;
            if( entity is IActive == false ) return null;
            if( this.OnBeforeChangeActiveState( entity, activeState ) )
            {
                ( entity as IActive ).IsActive = activeState;
                _db.SaveChanges();
            }
            return entity;
        }

        /// <summary>
        /// Gets the entity collection managed by this class
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public DbSet<TEntity> GetCollection()
        {

            var dbType = _db.GetType();
            var propertyInfo = dbType.GetProperties().FirstOrDefault( p => p.PropertyType == typeof( DbSet<TEntity> ) );
            if( propertyInfo == null ) return null;

            return propertyInfo.GetValue( _db ) as DbSet<TEntity>;

        }

        /// <summary>
        /// Returns the name of the table in the database that this class is managing
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            return typeof( TEntity ).Name;
        }

        /// <summary>
        /// Occurs before the active deliveredState is changed on an newEntity that implements IActive.  If the implementation
        /// returns false, the change will not be saved.  Gives the inheriting manager an opportunity to perform
        /// further work on the newEntity before it is saved.
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="activeState"></param>
        /// <returns></returns>
        public virtual bool OnBeforeChangeActiveState( TEntity entity, bool activeState )
        {
            return true;
        }

        /// <summary>
        /// Occurs after the manager creates a new newEntity, but before the database context is saved and the
        /// new newEntity is persisted.  This allows you to make changes to the newEntity, or perform further 
        /// database work before the context is saved
        /// </summary>
        /// <param name="newEntity"></param>
        public virtual void OnAfterCreateEntity( TEntity entity )
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs before the model is mapped onto the newEntity, for both Create and Update events
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newEntity"></param>
        public virtual void OnBeforeBind( TModel model, TEntity entity )
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs after the model is mapped onto the newEntity, for both Create and Update events
        /// </summary>
        /// <param name="newEntity"></param>
        public virtual void OnAfterBind( TModel model, TEntity entity )
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs immedietly before the database context is saved, for both Create and Update events
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="context"></param>
        public virtual void OnBeforeSaveEntity( TEntity entity )
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs immedietly after the database context is saved, for both Create and Update events
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="saveState"></param>
        /// <param name="context"></param>
        /// <param name="isCreateAction"></param>
        public virtual void OnAfterSaveEntity( TEntity entity, SaveState saveState, SaveContext saveContext )
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs after the originalEntity is mapped onto the newEntity during a copy operation
        /// </summary>
        /// <param name="newEntity"></param>
        public virtual void OnAfterCopy( TEntity newEntity, TEntity originalEntity )
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new instance of TEntity by making a copy of originalEntity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="newEntity"></param>
        public virtual SaveState Copy( TEntity originalEntity )
        {

            try
            {

                /* Create the newEntity and save the changes */
                var beforeSaveCollection = this.GetCollection();
                var beforeSaveEntity = new TEntity();

                // When mapping the original entity, copy everything but the primary key
                var keyProperty = this.GetEntityPrimaryKey( beforeSaveEntity );
                AutoMap.Map( originalEntity, beforeSaveEntity, new string[] { keyProperty.Name } );

                // Try to change the name of the entity to "Copy of..."
                var entityName = beforeSaveEntity.GetPropertyWithAttribute<EntityNameAttribute>();
                if( entityName != null && entityName.PropertyType == typeof( string ) )
                {
                    var copiedName = entityName.GetValue( beforeSaveEntity );
                    entityName.SetValue( beforeSaveEntity, string.Format( "Copy of {0}", copiedName ) );
                }

                // Add the copy to the collection and raise the create, copy and save events in case there is more 
                // work that needs to be done in the specific manager.
                beforeSaveCollection.Add( beforeSaveEntity );
                this.OnAfterCreateEntity( beforeSaveEntity );
                this.OnAfterCopy( beforeSaveEntity, originalEntity );
                this.OnBeforeSaveEntity( beforeSaveEntity );

                _db.SaveChanges();
                _db.Dispose();

                /* Create a new context and retrieve the new newEntity (otherwise navigation properties will be null) */
                _db = new TContext();
                var primaryKey = this.GetEntityPrimaryKeyValue( beforeSaveEntity );
                var afterSaveCollection = this.GetCollection();
                var afterSaveEntity = afterSaveCollection.Find( primaryKey );
                var saveState = SaveState.Success( afterSaveEntity, primaryKey );
                this.OnAfterSaveEntity( afterSaveEntity, saveState, SaveContext.Create );
                return saveState;

            }

            catch
            {
                var saveState = SaveState.Error;
                this.OnAfterSaveEntity( null, saveState, SaveContext.Create );
                return saveState;
            }

        }

        /// <summary>
        /// Performs a search against the associated data set using Solr.
        /// If Solr has not been set up for this entity, then it will yield no results.
        /// Returns the key values only.
        /// </summary>
        //public virtual List<TKey> SearchForEntityIds( string searchTerm )
        //{

        //    var result = SolrCore.Search<TEntity>( searchTerm );

        //    //if( result.Failed )
        //    //{
        //    //    throw new Exception( "Search Failed: " + result.SearchOptions.Type );
        //    //}

        //    return result.Keys<TKey>();

        //}

        /// <summary>
        /// Performs a search against the associated data set using Solr.
        /// If Solr has not been set up for this entity, then it will yield no results.
        /// </summary>
        //public virtual List<TEntity> SearchForEntities( string searchTerm )
        //{
        //    return this.FindWithPrimaryKeys( SearchForEntityIds( searchTerm ) );
        //}
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        /// <summary>
        /// Creates a new newEntity, adds it to its collections and saves the results
        /// </summary>
        /// <returns></returns>
        protected virtual SaveState CreateEntity()
        {
            try
            {

                /* Create the newEntity and save the changes */
                var beforeSaveCollection = this.GetCollection();
                var beforeSaveEntity = this.NewEntity();
                beforeSaveCollection.Add( beforeSaveEntity );
                this.OnAfterCreateEntity( beforeSaveEntity );
                this.OnBeforeSaveEntity( beforeSaveEntity );
                _db.SaveChanges();
                _db.Dispose();

                /* Create a new context and retrieve the new newEntity (otherwise navigation properties will be null) */
                _db = new TContext();
                var primaryKey = this.GetEntityPrimaryKeyValue( beforeSaveEntity );
                var afterSaveCollection = this.GetCollection();
                var afterSaveEntity = afterSaveCollection.Find( primaryKey );
                var saveState = SaveState.Success( afterSaveEntity, primaryKey );
                this.OnAfterSaveEntity( afterSaveEntity, saveState, SaveContext.Create );
                return saveState;

            }

            catch
            {
                var saveState = SaveState.Error;
                this.OnAfterSaveEntity( null, saveState, SaveContext.Create );
                return saveState;
            }
        }

        /// <summary>
        /// Retrieves an newEntity using the model's primary key, updates it and saves the results
        /// </summary>
        /// <returns></returns>
        protected virtual SaveState UpdateEntity()
        {
            try
            {

                var entity = this.Find( this.GetModelPrimaryKeyValue() );
                if( entity == null ) return SaveState.Error;
                this.Bind( entity );
                this.OnBeforeSaveEntity( entity );
                _db.SaveChanges();

                var primaryKey = this.GetEntityPrimaryKeyValue( entity );
                var saveState = SaveState.Success( entity, primaryKey );
                this.OnAfterSaveEntity( entity, saveState, SaveContext.Update );
                return saveState;

            }

            catch( Exception )
            {
                var saveState = SaveState.Error;
                this.OnAfterSaveEntity( null, saveState, SaveContext.Update );
                return saveState;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Finds the newEntity with the specified primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        private TEntity FindWithPrimaryKey( dynamic primaryKey )
        {
            var collection = this.GetCollection();
            return collection.Find( primaryKey );
        }

        /// <summary>
        /// Finds the entities that match the specified keys.  The result will be made by batching up the 
        /// primaryKeys into groups on 'n keys (def. 100) and making a single IN() select query to the database 
        /// for each batch, combining the results at the end.
        /// </summary>
        private List<TEntity> FindWithPrimaryKeys( List<TKey> primaryKeys )
        {
            
            var collection = this.GetCollection();
            var entities = new List<TEntity>();
            var key = this.GetEntityPrimaryKeyName();
            var query = string.Format( "@0.Contains(outerIt.{0})", key );
            var batchSize = 100;

            for( var i = 0; i < primaryKeys.Count; i += batchSize )
            {
                var keys = primaryKeys.Skip( i ).Take( batchSize );
                entities.AddRange( collection.Where( query, keys ) );
            }

            return entities;

        }

        /// <summary>
        /// Gets the primary key of the model.  The model must have a property that is decorated with KeyAttribute.
        /// </summary>
        /// <returns></returns>
        private PropertyInfo GetModelPrimaryKey()
        {
            var primaryKey = this.Model.GetPropertyWithAttribute<KeyAttribute>();
            if( primaryKey == null )
            {
                throw new Exception( "Model does not have a property that is decorated with the \"Key\" attribute.  You must decorate the primary key property of the model with the \"Key\" attribute before attempting to save it." );
            }
            return primaryKey;
        }

        /// <summary>
        /// Gets the primary key of the entity.  The entity must have a property that is decorated with KeyAttribute.
        /// </summary>
        /// <returns></returns>
        private PropertyInfo GetEntityPrimaryKey( TEntity entity )
        {
            var primaryKey = entity.GetPropertyWithAttribute<KeyAttribute>();
            if( primaryKey == null )
            {
                throw new Exception( "Entity does not have a property that is decorated with the \"Key\" attribute.  You must decorate the primary key property of the entity with the \"Key\" attribute before attempting to save it." );
            }
            return primaryKey;
        }

        /// <summary>
        /// Gets the value of the primary key property on the model
        /// </summary>
        /// <returns></returns>
        private dynamic GetModelPrimaryKeyValue()
        {
            var primaryKeyValue = this.GetModelPrimaryKey().GetValue( this.Model );
            if( primaryKeyValue == null ) return null;
            return (TKey)primaryKeyValue;
        }

        /// <summary>
        /// Gets the name of the primary key property on the entity
        /// </summary>
        /// <returns></returns>
        private string GetEntityPrimaryKeyName()
        {
            var stub = new TEntity();
            var primaryKeyValue = this.GetEntityPrimaryKey( stub );
            if( primaryKeyValue == null ) return null;
            return primaryKeyValue.Name;
        }

        /// <summary>
        /// Gets the value of the primary key property on the entity
        /// </summary>
        /// <returns></returns>
        private dynamic GetEntityPrimaryKeyValue( TEntity entity )
        {
            var primaryKeyValue = this.GetEntityPrimaryKey( entity ).GetValue( entity );
            if( primaryKeyValue == null ) return null;
            return (TKey)primaryKeyValue;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public TModel Model { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public static CacheBase<TManager, TEntity, TModel, TKey> Cache
        {
            get
            {
                if( _cache == null )
                {
                    _cache = new CacheBase<TManager, TEntity, TModel, TKey>();
                }
                return _cache;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
