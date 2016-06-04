using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Deletes an entity from the database context
        /// </summary>
        public static bool Delete<TEntity>( this DbSet<TEntity> dbSet, Guid recordId ) where TEntity : class, new()
        {
            
            try
            {

                var context = dbSet.GetContext();
                var entityType = typeof( TEntity );
                TEntity entity = new TEntity();
                var tableName = context.GetTableName<TEntity>();
                var keyProperty = entityType.GetPropertyWithAttribute<KeyAttribute>( entityType );

                context.Database.ExecuteSqlCommand( string.Format( "DELETE FROM {0} WHERE {1} = '{2}'", tableName, keyProperty.Name, recordId.ToString() ) );

                return true;

            }

            catch( Exception )
            {
                return false;
            }

        }

        /// <summary>
        /// Deletes an entity from the database context
        /// </summary>
        public static bool Delete<TEntity>( this DbSet<TEntity> dbSet, int recordId ) where TEntity : class, new()
        {

            try
            {

                var context = dbSet.GetContext();
                var entityType = typeof( TEntity );
                TEntity entity = new TEntity();
                var tableName = context.GetTableName<TEntity>();
                var keyProperty = entityType.GetPropertyWithAttribute<KeyAttribute>( entityType );

                context.Database.ExecuteSqlCommand( string.Format( "DELETE FROM {0} WHERE {1} = {2}", tableName, keyProperty.Name, recordId.ToString() ) );

                return true;

            }

            catch( Exception )
            {
                return false;
            }

        }

        /// <summary>
        /// Deletes a range of entities from the database context.  Entities will be deleted in batches to avoid creating
        /// SQL commands that are too large
        /// </summary>
        public static bool Delete<TEntity>( this DbSet<TEntity> dbSet, IEnumerable<Guid> recordIds ) where TEntity : class, new()
        {

            if( recordIds == null || recordIds.Count() == 0 ) return true;

            try
            {

                var context = dbSet.GetContext();
                var entityType = typeof( TEntity );
                TEntity entity = new TEntity();
                var tableName = context.GetTableName<TEntity>();
                var keyProperty = entityType.GetPropertyWithAttribute<KeyAttribute>( entityType );

                var batchSize = 100;
                var completed = 0;
                var batch = recordIds.Skip( completed ).Take( batchSize );

                while( batch.Count() > 0 )
                {
                    context.Database.ExecuteSqlCommand( string.Format( "DELETE FROM {0} WHERE {1} IN('{2}')", tableName, keyProperty.Name, String.Join( "','", batch ) ) );
                    completed += batchSize;
                    batch = recordIds.Skip( completed ).Take( batchSize );
                }

                return true;


            }

            catch( Exception )
            {
                return false;
            }

        }

        /// <summary>
        /// Deletes a range of entities from the database context.  Entities will be deleted in batches to avoid creating
        /// SQL commands that are too large
        /// </summary>
        public static bool Delete<TEntity>( this DbSet<TEntity> dbSet, IEnumerable<int> recordIds ) where TEntity : class, new()
        {

            if( recordIds == null || recordIds.Count() == 0 ) return true;

            try
            {

                var context = dbSet.GetContext();
                var entityType = typeof( TEntity );
                TEntity entity = new TEntity();
                var tableName = context.GetTableName<TEntity>();
                var keyProperty = entityType.GetPropertyWithAttribute<KeyAttribute>( entityType );

                var batchSize = 100;
                var completed = 0;
                var batch = recordIds.Skip( completed ).Take( batchSize );

                while( batch.Count() > 0 )
                {
                    context.Database.ExecuteSqlCommand( string.Format( "DELETE FROM {0} WHERE {1} IN({2})", tableName, keyProperty.Name, String.Join( ",", batch ) ) );
                    completed += batchSize;
                    batch = recordIds.Skip( completed ).Take( batchSize );
                }

                return true;

            }

            catch( Exception )
            {
                return false;
            }

        }

        public static DbContext GetContext<TEntity>( this DbSet<TEntity> dbSet ) where TEntity : class
        {
            object internalSet = dbSet
                .GetType()
                .GetField( "_internalSet", BindingFlags.NonPublic | BindingFlags.Instance )
                .GetValue( dbSet );
            object internalContext = internalSet
                .GetType()
                .BaseType
                .GetField( "_internalContext", BindingFlags.NonPublic | BindingFlags.Instance )
                .GetValue( internalSet );
            return (DbContext)internalContext
                .GetType()
                .GetProperty( "Owner", BindingFlags.Instance | BindingFlags.Public )
                .GetValue( internalContext, null );
        }

        /// <summary>
        /// Performs a 'safe' join.
        /// Useful for when there are large datasets that need to be joined.
        /// </summary>
        /// <remarks>
        /// Overcomes the issue where joining with a DbSet could cause a stack overflow exception.
        /// </remarks>
        public static List<TResult> SafeJoin<TEntity, TInner, TKey, TResult>( this DbSet<TEntity> dbSet, IEnumerable<TInner> inner, Expression<Func<TEntity, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TEntity,TInner,TResult>> resultSelector ) where TEntity : class
        {
            List<TResult> list = new List<TResult>();

            int skip = 200;
            int loops = inner.Count() / skip + 1;

            for ( int j = 0 ; j < loops ; j++ )
            {
                list.AddRange( dbSet.Join( inner.Skip( j * skip ).Take( skip ), outerKeySelector, innerKeySelector, resultSelector ) );
            }

            return list;
        }
    
    }
}
