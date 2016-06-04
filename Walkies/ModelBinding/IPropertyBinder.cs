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
    /// Used to aid the extended (custom) model binder in completing it's job.
    /// </summary>
    /// <remarks>
    /// Recommended attribute:
    /// [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
    /// </remarks>
    public interface IPropertyBinder
    {
        BindingResult BindProperty( string attemptedValue );

        void SetValue( ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value );
    }
}
