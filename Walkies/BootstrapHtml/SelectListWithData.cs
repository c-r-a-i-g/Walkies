using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Walkies.BootstrapHtml
{
    public class SelectListWithData : List<SelectListItemWithData>
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public SelectListWithData( IEnumerable items, string textField, string valueField, string dataProperties, object selectedValue = null )
        {

            var selectedValues = ( ( selectedValue != null ) ? new List<string> { Convert.ToString( selectedValue, CultureInfo.CurrentCulture ) } : new List<string>() );

            foreach( var item in items )
            {
                
                var value = Eval( item, valueField );
                var properties = dataProperties.Split( ',' );
                var attributes = new Dictionary<string, string>();

                foreach( var property in properties )
                {
                    var propertyValue = Eval( item, property );
                    attributes.Add( property, propertyValue );
                }

                this.Add( new SelectListItemWithData() 
                {
                    Text = Eval( item, textField ),
                    Value = value,
                    Attributes = attributes,
                    Selected = selectedValues.Contains( value )
                });
            }
        
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

        private string Eval( object container, string expression )
        {
            object value = container;
            if( !String.IsNullOrEmpty( expression ) )
            {
                value = DataBinder.Eval( container, expression );
            }
            return Convert.ToString( value, CultureInfo.CurrentCulture );
        }
        
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
