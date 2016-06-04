using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Walkies.Framework.Interfaces;
using Newtonsoft.Json;

namespace Walkies.Framework.BaseClasses
{
    public abstract class DataPageModelBase<TManager,TModel,TEntity,TKey> : PageModelBase, IDataPageModel<TManager,TEntity,TKey> 
        where TManager : IDataManager<TModel,TEntity,TKey>, new() 
        where TModel : class 
        where TEntity : class
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// Creates an empty instance of the newEntity
        /// </summary>
        public DataPageModelBase() 
        {
            this.Entity = null;
            this.IsActive = true;
        }

        /// <summary>
        /// Creates a new instance of the newEntity, filled with data from the newEntity found using the specified primaryKey
        /// </summary>
        /// <param name="primaryKey"></param>
        public DataPageModelBase( TKey primaryKey ) : this()
        {
            this.Bind( primaryKey );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Binds the specified entity onto the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity"></param>
        public void Bind( TKey primaryKey )
        {

            var manager = new TManager() { Model = this as TModel };
            var entity = manager.Find( primaryKey );

            if( entity != null )
            {
                this.Bind( entity );
            }

            this.Entity = entity;

        }

        /// <summary>
        /// Binds the specified entity onto the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Bind( TEntity entity )
        {
            this.OnBeforeBind( entity );
            AutoMap.Map( entity, this );
            this.OnAfterBind( entity );
        }

        /// <summary>
        /// Binds this entity onto the specified entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity"></param>
        public void BindTo( TEntity entity )
        {
            AutoMap.Map( this as TModel, entity );
        }

        /// <summary>
        /// Returns a new manager class for this entity
        /// </summary>
        /// <returns></returns>
        public TManager GetManager()
        {
            return new TManager() { Model = this as TModel };
        }

        /// <summary>
        /// Returns the entity identified by the specified key
        /// </summary>
        /// <returns></returns>
        public TEntity GetEntity( TKey primaryKey )
        {
            var manager = this.GetManager();
            return manager.Find( primaryKey );
        }

        /// <summary>
        /// Saves the entity instance by creating an instance of its manager class and executing its Save function
        /// </summary>
        public SaveState Save()
        {

            var manager = new TManager() { Model = this as TModel };
            var result = manager.Save();

            if( result.Entity == null && result.IsSuccessful )
            {
                throw new Exception( "SaveState.Entity was not set during the Data Manager classes Save() function.  The Entity is used by other processes and must be set on a successful Save() result." );
            }

            this.Entity = result.Entity;
            this.OnModelSaved( result );

            return result;

        }

        /// <summary>
        /// Occurs before the entity is bound to the newEntity.  This can be overriden in the inheriting class to perform work
        /// on the newEntity before the binding takes place
        /// </summary>
        /// <param name="newEntity"></param>
        public virtual void OnBeforeBind( TEntity entity )
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs after the entity is bound to the newEntity.  This can be overriden in the inheriting class to perform work
        /// on the entity after the binding takes place
        /// </summary>
        /// <param name="newEntity"></param>
        public virtual void OnAfterBind( TEntity entity )
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Occurs after the models manager has saved the entity.  This can be overriden in the inheriting class to 
        /// allow the entity to inspect the result of the save and adjust itself before returning to the view.
        /// </summary>
        /// <param name="saveState"></param>
        public virtual void OnModelSaved( SaveState saveState )
        {
            // throw new NotImplementedException();
        }

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

        [JsonIgnore]
        public TEntity Entity { get; private set; }

        [Required, DisplayName( "Is Active" )]
        [JsonProperty( "isActive" )]
        public bool IsActive { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets the name of the entity that is being modelled
        /// </summary>
        [JsonProperty( "entityName" )]
        public string EntityName
        {
            get
            {
                return typeof( TEntity ).Name;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
