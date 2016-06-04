using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.BootstrapHtml
{

    public enum ButtonStyle
    {

        [Description( "" )]
        None = 0,

        [Description( "btn btn-default" )]
        Default = 1,

        [Description( "btn btn-primary" )]
        Primary = 2,

        [Description( "btn btn-success" )]
        Success = 3,

        [Description( "btn btn-info" )]
        Info = 4,

        [Description( "btn btn-warning" )]
        Warning = 5,
        
        [Description( "btn btn-danger" )]
        Danger = 6,

        [Description( "btn btn-link" )]
        Link = 7,

    }
}
