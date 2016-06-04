using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Walkies.Framework.Web.DataTables;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Session;

namespace Walkies.Framework.BaseClasses
{
    public abstract class ListedDataPageModelBase<TManager,TModel,TEntity,TKey> : DataTablesRequestModel, IEditorAttributes, IListedDataPageModel<TManager,TEntity> where TManager : class, IDataManager<TModel,TEntity,TKey>, new() where TEntity : class where TModel : class, IDataPageModel<TManager,TEntity,TKey>
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private FilteredList<TEntity> _filteredList = null;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Returns a new manager class for this newEntity
        /// </summary>
        /// <returns></returns>
        public TManager GetManager()
        {
            return new TManager();
        }

        /// <summary>
        /// When overriden in the derived class, creates an filterQuery that will be used to filter the 
        /// GetList function on the models manager class specific to filtering to the groups that the user
        /// is allowed to access
        /// </summary>
        /// <returns></returns>
        public virtual Expression<Func<TEntity, bool>> OnCreateAuthorisationExpression()
        {
            return x => true;
        }

        /// <summary>
        /// When overriden in the derived class, creates an filterQuery that will be used to filter the 
        /// GetList function on the models manager class
        /// </summary>
        /// <returns></returns>
        public virtual Expression<Func<TEntity, bool>> OnCreateFilterExpression()
        {
            return x => true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual OrderByExpression OnCreateOrderExpression()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual object OnCreateTableRow( TEntity entity )
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual void OnCreateTableRow( IEnumerable<TEntity> items )
        {
            
        }

        /// <summary>
        /// Gets a data tables response for the specified request.  HasPriviledge be overridden in the derived class
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual DataTablesResponseModel GetDataTableResponse()
        {

            this.GetItems();
            this.TotalCount = _filteredList.TotalCount;
            this.FilteredCount = _filteredList.FilteredCount;

            // retrieve a paginated set of entities to render
            var tableData = new List<object>();
            var entitiesToRender = this.Items.Skip( this.Start ).Take( this.Length );

            // loop over the entities that are to be rendered and create an anonymous object that contains
            // the row data.  The row data will be serialised to json objects and returned to the client.
            foreach( var entity in entitiesToRender.ToList() )
            {
                var rowData = this.OnCreateTableRow( entity );
                if( rowData != null )
                {
                    tableData.Add( rowData );
                }
            }

            return new DataTablesResponseModel( this, this.TotalCount, this.FilteredCount, tableData );

        }

        /// <summary>
        /// Creates the editor path to be used based off the URL absolute path. If the current path doesn't end in a slash (/), then one is automatically added.
        /// </summary>
        /// <param name="request">The current request object</param>
        /// <param name="path">The path to continue to</param>
        /// <returns></returns>
        public string CreateEditorPath( HttpRequestBase request, string path )
        {
            string url = request.Url.AbsolutePath;

            if ( url.EndsWith( "/" ) == false )
            {
                url += "/";
            }

            url += path;

            return url;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected Expression<Func<TSource, dynamic>> GetPropertyExpression<TSource>( string propertyName )
        {
            var param = Expression.Parameter( typeof(TSource), "x" );
            var propertyInfo = typeof( TSource ).GetProperty( propertyName );
            var conversion = Expression.Convert( Expression.Property( param, propertyName ), propertyInfo.PropertyType );
            return Expression.Lambda<Func<TSource, dynamic>>( conversion, param );
        }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        // Uses the derived class' query filterQuery to filter the list
        private FilteredList<TEntity> GetItems()
        {

            if( _filteredList != null )
            {
                return _filteredList;
            }

            var manager = this.GetManager();
            var groupQuery = this.OnCreateAuthorisationExpression();
            var filterQuery = this.OnCreateFilterExpression();
            var order = this.OnCreateOrderExpression();

            _filteredList = manager.GetFilteredList( groupQuery, filterQuery, order );
            return _filteredList;

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public string EditorPath { get; set; }
        public bool CanCopyRecords { get; set; }
        public bool HasCustomOptions { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets the name of the entity that is being listed
        /// </summary>
        public string EntityName
        {
            get
            {
                return typeof( TEntity ).Name;
            }
        }

        public List<TEntity> Items
        {
            get
            {
                return this.GetItems().Items.ToList();
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
