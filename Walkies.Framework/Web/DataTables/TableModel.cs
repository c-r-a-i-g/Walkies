using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Session;

namespace Walkies.Framework.Web.DataTables
{
    public class TableModel
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public TableModel() { }

        public TableModel( IPageModel pageModel, HelperResult headerTemplate, HelperResult tableData = null )
        {

            bool displayOptionsColumn = false;

            this.PageModel = pageModel;
            this.HeaderTemplate = headerTemplate;
            this.TableData = tableData;

            if( pageModel is IEditorAttributes )
            {
                var editorAttributes = pageModel as IEditorAttributes;
                this.EditorPath = editorAttributes.EditorPath;
                this.EntityName = editorAttributes.EntityName;
                if( editorAttributes.CanCopyRecords || editorAttributes.HasCustomOptions ) displayOptionsColumn = true;
            }

            this.DisplayOptionsColumn = displayOptionsColumn;// || UserSession.Can( Priviledge.ChangeEntityStates );

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

        public IPageModel PageModel { get; set; }
        public HelperResult HeaderTemplate { get; set; }
        public HelperResult TableData { get; set; }
        public string EditorPath { get; set; }
        public string EntityName { get; set; }
        public bool DisplayOptionsColumn { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public string TableId
        {
            get
            {
                return this.PageModel.PageTitle.Replace( "Manage", "" ).Trim().Replace( " ", "-" ).ToLower() + "-table";
            }
        }

        public bool HasTableData
        {
            get
            {
                return this.TableData != null;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
