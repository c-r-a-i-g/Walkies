using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Database.Entities;
using Walkies.Database.Interfaces;

namespace Walkies.Database
{
    public class WalkiesDB : DbContext
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public WalkiesDB() : base( "WalkiesDB" )
        {

            base.Configuration.LazyLoadingEnabled = true;

            var objectContext = ( (IObjectContextAdapter)this ).ObjectContext;
            objectContext.ObjectMaterialized += ( sender, e ) =>
            {
                if( e.Entity is IOnMaterialise )
                {
                    ( e.Entity as IOnMaterialise ).OnMaterialise();
                }
            };

        #if DEBUG
            this.Database.Log = ( text ) => Debug.WriteLine( text );
        #endif

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Tables

        /* -----------------------------------------------
         * AspNet Security
         */
        public DbSet<AspNetUser> AspNetUsers { get; set; }

        /* -----------------------------------------------
         * Groups & Users
         */
        public DbSet<User> Users { get; set; }


        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Whenever an entity is added or modified, check to see if it implements OnBeforeSave, and if
        /// it does, call its OnBeforeSave function
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {

            var entities = ChangeTracker.Entries<IOnBeforeSave>()
                                        .Where( p => p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Unchanged )
                                        .Select( p => p.Entity )
                                        .Cast<IOnBeforeSave>();

            foreach( var entity in entities )
            {
                entity.OnBeforeSave();
            }

            return base.SaveChanges();

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        /// <summary>
        /// Configures the model relationships
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {

        }

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
