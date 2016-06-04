using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walkies
{
    /// <summary>
    /// Tells the AutoMap to map this property to a property with a specific name on the destination.
    /// </summary>
    public class MapsToAttribute : Attribute
    {

        /// <summary>
        /// The name of the property on the destination to map to.
        /// </summary>
        public string MapsTo { get; set; }

        /// <summary>
        /// Tells the AutoMap to map this property to a property with a specific name on the destination.
        /// </summary>
        /// <param name="mapsTo">The name of the property on the destination to map to</param>
        public MapsToAttribute( string mapsTo )
        {
            MapsTo = mapsTo;
        }

    }
}