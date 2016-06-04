using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    /// <summary>
    /// Tells the AutoMap to map to this property from a property on the source with a specific name.
    /// </summary>
    public class MapsFromAttribute : Attribute
    {

        /// <summary>
        /// The name of the property on the source to map from.
        /// </summary>
        public string MapsFrom { get; set; }

        /// <summary>
        /// Tells the AutoMap to map to this property from a property on the source with a specific name.
        /// </summary>
        /// <param name="mapsTo">The name of the property on the source to map from</param>
        public MapsFromAttribute( string mapsTo )
        {
            MapsFrom = mapsTo;
        }

    }
}
