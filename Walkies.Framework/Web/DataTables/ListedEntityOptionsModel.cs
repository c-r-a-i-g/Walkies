using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Walkies.Framework.Interfaces;

namespace Walkies.Framework.Web.DataTables
{
    public class ListedEntityOptionsModel
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// 
        /// </summary>
        public ListedEntityOptionsModel() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editorAttributes"></param>
        /// <param name="entityOptions"></param>
        public ListedEntityOptionsModel( IEditorAttributes editorAttributes, HelperResult entityOptions = null )
        {
            this.Intialise( editorAttributes, entityOptions );
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

        private void Intialise( IEditorAttributes editorAttributes, HelperResult entityOptions = null )
        {

            this.EntityOptions = entityOptions;
            this.CanCopyRecords = editorAttributes.CanCopyRecords;

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public HelperResult EntityOptions { get; set; }
        public bool CanCopyRecords { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public bool HasEntityOptions
        {
            get { return this.EntityOptions != null; }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
