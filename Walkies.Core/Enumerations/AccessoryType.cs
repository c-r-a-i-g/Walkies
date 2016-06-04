using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    public enum AccessoryType
    {
        [Description( "Factory Options" )]
        FactoryOption = 1,

        [Description( "Genuine Accessories" )]
        Genuine = 2,

        [Description( "Non Genuine Accessories" )]
        NonGenuine = 3,

        [Description( "Aftermarket Accessories" )]
        Aftermarket = 4
    }
}
