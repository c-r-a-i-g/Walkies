using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Walkies
{
    /// <summary>
    /// An Walkies extension to the model binder that will enable custom property binding attributes to be applied.
    /// </summary>
    public class ExtendedModelBinder : DefaultModelBinder
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        /// <summary>
        /// Searches for the 'IPropertyBinder' attribute on the provided property. If one is provided, the binder on that attribute will be used instead of the default binder.
        /// </summary>
        protected override void BindProperty( ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor )
        {
            IPropertyBinder customPropertyBinder = propertyDescriptor.Attributes[ typeof( IPropertyBinder ) ] as IPropertyBinder;

            if ( customPropertyBinder != null )
            {
                string propertyName = propertyDescriptor.Name;

                if ( string.IsNullOrEmpty( bindingContext.ModelName) == false )
                {
                    propertyName = bindingContext.ModelName + "." + propertyName;
                }

                ValueProviderResult valueProvider = bindingContext.ValueProvider.GetValue( propertyName );

                if ( valueProvider == null ) return;

                BindingResult result = customPropertyBinder.BindProperty( valueProvider.AttemptedValue );

                if ( result.IsSuccessful )
                {
                    ValueProviderResult bindResult = new ValueProviderResult( valueProvider.RawValue, result.Value.ToString(), valueProvider.Culture );

                    if ( bindingContext.ModelState[ propertyDescriptor.Name ] == null )
                    {
                        bindingContext.ModelState.Add( propertyDescriptor.Name, new ModelState() { Value = bindResult } );
                    }
                    else
                    {
                        bindingContext.ModelState[ propertyDescriptor.Name ].Value = bindResult;
                    }

                    customPropertyBinder.SetValue( bindingContext, propertyDescriptor, result.Value );
                }
                else
                {
                    bindingContext.ModelState.AddModelError( propertyDescriptor.Name, result.ErrorMessage );
                }
            }
            else
            {
                base.BindProperty( controllerContext, bindingContext, propertyDescriptor );
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
