using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    /// <summary>
    /// Tells the AutoMap to create a mapping definition for this property between the property's type and the destination type.
    /// </summary>
    public class CreateChildMapToAttribute : Attribute
    {

        /// <summary>
        /// The property type of the destination.
        /// </summary>
        public Type DestinationType { get; set; }

        /// <summary>
        /// Tells the AutoMap to create a mapping definition for this property between the property's type and the destination type.
        /// </summary>
        /// <param name="destinationType">The property type of the destination</param>
        public CreateChildMapToAttribute( Type destinationType )
        {
            DestinationType = destinationType;
        }

    }
}
