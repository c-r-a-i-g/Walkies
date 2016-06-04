using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Postal;

using Walkies.Framework.BaseClasses;

namespace Walkies.Framework.Models.Emails
{
    public class WelcomeEmailModel : UserEmailModelBase
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public WelcomeEmailModel( Guid userId, string userName, string password, bool isEmailCredential ) : base( "WelcomeEmail", userId )
        {
            this.UserName = userName;
            this.TemporaryPassword = password;
            this.IsEmailCredential = isEmailCredential;
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

        public string UserName { get; set; }
        public string TemporaryPassword { get; set; }
        public bool IsEmailCredential { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
