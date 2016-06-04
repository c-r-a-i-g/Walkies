using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DealerCell.Platform.Database.Entities;
using DealerCell.Platform.Database.Interfaces;

namespace DealerCell.Platform.Database
{
    public class DealerCellDB : DbContext
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public DealerCellDB() : base( "DealerCellDB" )
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
        public DbSet<Client> Clients { get; set; }
        public DbSet<Dealership> Dealerships { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupDepartment> StructureNodes { get; set; }
        public DbSet<DomainSetting> DomainSettings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserPin> Pins { get; set; }
        public DbSet<UserBookmark> Bookmarks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        /* -----------------------------------------------
         * Deals
         */
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealView> DealViews { get; set; }

        /* -----------------------------------------------
         * Packages
         */
        public DbSet<PackageTemplate> PackageTemplates { get; set; }
        public DbSet<Package> Packages { get; set; }

        /* -----------------------------------------------
         * Vehicles
         */
        public DbSet<Vehicle> Vehicles { get; set; }

        /* -----------------------------------------------
         * Ipps
         */
        public DbSet<Ipp> Ipps { get; set; }

        /* -----------------------------------------------
         * Integration Hub
         */
        public DbSet<HubTransaction> HubTransactions { get; set; }

        /* -----------------------------------------------
         * Other
         */
        public DbSet<Salesperson> Salespersons { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

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

            modelBuilder.Entity<Group>()
                        .HasOptional( c => c.Parent )
                        .WithMany()
                        .HasForeignKey( c => c.ParentGroupId );

            modelBuilder.Entity<Group>()
                        .HasMany( e => e.Children )
                        .WithOptional( m => m.Parent );

            modelBuilder.Entity<Group>()
                        .HasMany( x => x.Departments )
                        .WithMany( x => x.Groups )
                        .Map( x =>
                        {
                            x.ToTable( "GroupDepartments" ); 
                            x.MapLeftKey( "GroupId" );
                            x.MapRightKey( "DepartmentId" );
                        } );

            modelBuilder.Entity<User>()
                        .HasMany( x => x.Dealerships )
                        .WithMany( x => x.Users )
                        .Map( x =>
                        {
                            x.ToTable( "UserDealership" );
                            x.MapLeftKey( "UserId" );
                            x.MapRightKey( "DealershipId" );
                        } );

            modelBuilder.Entity<User>()
                        .HasMany( u => u.AssignedDeals )
                        .WithOptional( d => d.AssignedToUser );

            modelBuilder.Entity<User>()
                        .HasMany( u => u.CreatedDeals )
                        .WithOptional( d => d.CreatedByUser );

            modelBuilder.Entity<User>()
                        .HasMany( u => u.SalespersonDeals )
                        .WithOptional( d => d.SalespersonUser );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.VehicleSalePrice )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.DealerCharges )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.FuelCost )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.DealerDiscount )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.StampDuty )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.RegistrationCost )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.CTPCost )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.TradeInPayout )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.TotalPriceBeforeRegistration )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.TotalAmountPayable )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.TotalGST )
                        .HasPrecision( 11, 2 );

            modelBuilder.Entity<Deal>()
                        .Property( s => s.TotalAccessoriesAndOptions )
                        .HasPrecision( 11, 2 );

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
