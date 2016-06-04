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
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
    public class CreateMapAttribute : Attribute
    {

        /// <summary>
        /// The property type of the source.
        /// </summary>
        public Type SourceType { get; set; }

        /// <summary>
        /// The property type of the destination.
        /// </summary>
        public Type DestinationType { get; set; }

        /// <summary>
        /// Tells the AutoMap to create a mapping definition for this property between the property's type and the source type.
        /// </summary>
        /// <param name="sourceType">The property type of the source</param>
        /// <param name="destinationType">The property type of the destination</param>
        public CreateMapAttribute( Type sourceType, Type destinationType )
        {
            SourceType = sourceType;
            DestinationType = destinationType;
        }

    }
}
