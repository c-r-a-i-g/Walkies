using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    /// <summary>
    /// Tells the AutoMap class to not map this attribute. This should be applied to the destination property, not the source property.
    /// </summary>
    public class AutoMapIgnoreAttribute : Attribute
    {

    }
}
