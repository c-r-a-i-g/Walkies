using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Walkies.Core.Enumerations;
using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Web.Session
{
    public class SecureData 
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public SecureData()
        {
            this.UserPermissions = new List<Permission>();
            this.DescendentGroupIds = new List<Guid>();
            this.AccessibleGroupIds = new List<Guid>();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Saves the state of the secure data
        /// </summary>
        public void Save()
        {
            HttpContext.Current.Session[ UserSession.SESSION_KEY ] = this;
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

        public List<Permission> UserPermissions { get; set; }
        public int RoleType { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<Guid> DescendentGroupIds { get; set; }
        public List<Guid> AccessibleGroupIds { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
