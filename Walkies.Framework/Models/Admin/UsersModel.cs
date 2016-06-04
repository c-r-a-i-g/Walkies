using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

using PredicateExtensions;

using Walkies.Database.Entities;
using Walkies.Framework.Managers;
using Walkies.Framework.Models.Admin;
using Walkies.Framework.BaseClasses;
using Walkies.Framework.Web;
using Walkies.Framework.Enumerations;
using Walkies.Framework.Web.DataTables;
using Walkies.Core.Enumerations;

namespace Walkies.Framework.Models.Admin
{
    public class UsersModel : ListedDataPageModelBase<UserManager, UserModel, User, Guid>
    {

        // --------------------------------------------------------------------------------------------------------

        #region Class Members

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Constructor and Intialisation

        public UsersModel()
        {
        }

        public UsersModel( Guid? clientId )
        {
            this.ClientId = clientId;
        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// Gets a filter expression that filters to the set of entities that the user is authorised to see.
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<User, bool>> OnCreateAuthorisationExpression()
        {

            Expression<Func<User, bool>> expression = x => true;

            return expression;

        }

        public override Expression<Func<User, bool>> OnCreateFilterExpression()
        {

            Expression<Func<User, bool>> expression = x => true;

            // The results need to be filtered to active state
            if( this.IsActive.HasValue )
            {
                expression = expression.And( x => x.IsActive == this.IsActive );
            }

            // There is a search term on the model, filter the filterQuery to the term
            if( this.HasSearch )
            {
                //var entityIds = this.GetManager().SearchForEntityIds( this.Search.Value );
                //expression = expression.And( x => entityIds.Contains( x.UserId ) );
            }

            return expression;

        }

        /// <summary>
        /// Creates the filterQuery that will order the table results
        /// </summary>
        /// <returns></returns>
        public override OrderByExpression OnCreateOrderExpression()
        {
            var direction = Direction.Ascending;
            Expression<Func<User, string>> expression = x => x.FirstName;
            return new OrderByExpression( expression, direction );
        }

        /// <summary>
        /// Implements the table row data object.  A collection of these will be returned as JSON to the datatable
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override object OnCreateTableRow( User entity )
        {
            return new
            {
                EntityId = entity.UserId.ToString(),
                Name = entity.FullName,
                Username = entity.UserName,
                LastLogin = entity.DateLastLogin.HasValue ? entity.DateLastLogin.Value.ToString( "dd/MM/yyyy" ) : "-",
                FailedLogins = entity.IsLocked ? "(LOCKED)" : ( entity.InvalidLoginAttempts == 0 ? "-" : entity.InvalidLoginAttempts.ToString() ),
                Options = "",
                IsLocked = entity.IsLocked,
                IsActive = entity.IsActive
            };
        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Private Methods

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Static Methods

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Properties

        /// <summary>
        /// Gets or sets the IsActive filter
        /// </summary>
        public bool? IsActive
        {
            get
            {
                return this.GetFilterValue<bool>( "IsActive" );
            }
            set
            {
                this.SetFilterValue( "IsActive", value );
            }
        }

        /// <summary>
        /// Gets or sets the ClientId filter
        /// </summary>
        public Guid? ClientId
        {
            get
            {
                return this.GetFilterValue<Guid>( "ClientId" );
            }
            set
            {
                this.SetFilterValue( "ClientId", value );
            }
        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Derived Properties

        #endregion

        // --------------------------------------------------------------------------------------------------------

    }
}