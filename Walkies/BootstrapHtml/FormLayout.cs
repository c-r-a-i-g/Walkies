using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.BootstrapHtml
{
    public class FormLayout
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public FormLayout()
        {
            FormStyle = FormStyle.Vertical;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public static FormLayout Vertical()
        {
            return new FormLayout()
            {
                FormStyle = FormStyle.Vertical,
                LabelClass = "",
                FieldClass = ""
            };
        }

        public static FormLayout Horizontal( string labelClass, string fieldClass )
        {
            return new FormLayout()
            {
                FormStyle = FormStyle.Horizontal,
                LabelClass = labelClass,
                FieldClass = fieldClass
            };
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

        public FormStyle FormStyle { get; set; }
        public string LabelClass { get; set; }
        public string FieldClass { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
