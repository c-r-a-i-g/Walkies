using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Database;
using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Models.Shared
{
    public class EntityStatusModel
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public EntityStatusModel()
        {
            this.Initialise();
        }

        public EntityStatusModel( IPageModel pageModel, bool isActive )
        {
            this.PageModel = pageModel;
            this.IsActive = isActive;
            this.Initialise();
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

        private void Initialise()
        {

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public IPageModel PageModel { get; private set; }
        public bool? IsActive { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public bool CanChangeActiveState
        {
            get
            {
                return this.IsActive.HasValue;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
