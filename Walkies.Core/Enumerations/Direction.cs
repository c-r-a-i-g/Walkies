using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    public enum Direction
    {
        [Description( "Ascending" )]
        Ascending = 0,

        [Description( "Descending" )]
        Descending = 1,
    }
}
