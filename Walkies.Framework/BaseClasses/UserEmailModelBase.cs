using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Database;
using Walkies.Database.Entities;
using Postal;

namespace Walkies.Framework.BaseClasses
{
    public class UserEmailModelBase : EmailModelBase
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public UserEmailModelBase( string viewName ) : base( viewName ) 
        { 
        }

        public UserEmailModelBase( string viewName, Guid userId ) : base( viewName )
        {

            var db = new WalkiesDB();
            var user = db.Users.Find( userId );
            if( user == null ) throw new ArgumentException( "The userId provided does not match a valid user." );

            AutoMap.Map( user, this );

            // Use the recipients theme, not the current users, otherwise the email may not be personalised
            // for the recipient.  The theme is used for appending attachments and styling.
            this.Theme = "_default";

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

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

        public string FirstName { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
