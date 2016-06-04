using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Enumerations
{
    public enum CellAlignment
    {

        [Description( "left" )]
        Left = 0,

        [Description( "right" )]
        Right = 1,

        [Description( "center" )]
        Center = 2
    }
}
