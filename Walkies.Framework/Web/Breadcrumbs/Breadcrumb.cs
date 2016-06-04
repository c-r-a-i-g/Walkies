using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Breadcrumbs
{
    public class Breadcrumb
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

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

        public string Text { get; set; }
        public string Url { get; set; }
        public string OnClick { get; set; }
        public string Class { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        [AutoMapIgnore]
        public BreadcrumbType BreadcrumbType
        {
            get
            {
                if( string.IsNullOrEmpty( this.Url ) == false )
                {
                    return BreadcrumbType.Link;
                }
                else if( string.IsNullOrEmpty( this.OnClick ) == false )
                {
                    return BreadcrumbType.ScriptLink;
                }
                else
                {
                    return BreadcrumbType.Label;
                }
            }
        }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
