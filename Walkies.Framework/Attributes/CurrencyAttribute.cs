using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Walkies;

namespace System.ComponentModel.DataAnnotations
{
    public class CurrencyAttribute : ValidationAttribute, IClientValidatable, IPropertyBinder
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members
            
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public CurrencyAttribute() : base()
        {
        }

        protected CurrencyAttribute( Func<string> errorMessageAccessor ) : base( errorMessageAccessor )
        {
        }

        protected CurrencyAttribute( string errorMessage ) : base( errorMessage )
        {
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public override bool IsValid( object value )
        {
            decimal decimalValue;

            if ( value == null )
            {
                return true;
            }

            return decimal.TryParse( value.ToString(), NumberStyles.Currency, CultureInfo.CurrentCulture, out decimalValue );
        }

        /// <summary>
        /// Attempts to bind the value as a decimal value.
        /// Capable of stripping out currency formatting.
        /// </summary>
        public BindingResult BindProperty( string attemptedValue )
        {
            if ( string.IsNullOrEmpty( attemptedValue ) )
            {
                return BindingResult.Success( "" );
            }

            decimal decimalValue;

            if ( decimal.TryParse( attemptedValue, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimalValue ) == false )
            {
                return BindingResult.Failed( "The value specified is not a valid currency number" );
            }

            return BindingResult.Success( decimalValue );
        }

        public void SetValue( ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value )
        {
            if ( value == null || ( value is string && string.IsNullOrEmpty( value as string ) ) )
            {
                return;
            }

            propertyDescriptor.SetValue( bindingContext.Model, value );
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules( ModelMetadata metadata, ControllerContext context )
        {
            ModelClientValidationRule newRule = new ModelClientValidationRule();

            newRule.ErrorMessage = string.Format( ErrorMessageString, metadata.GetDisplayName() );
            newRule.ValidationType = "currency";
            newRule.ValidationParameters.Add( "symbol", "$" );
            newRule.ValidationParameters.Add( "soft", false );

            return new[]
            {
                newRule
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

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
