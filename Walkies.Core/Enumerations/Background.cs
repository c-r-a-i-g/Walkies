using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    public enum Background
    {

        [Description( "background-default" )]
        Default,

        [Description( "background-login" )]
        Login,

        [Description( "background-admin" )]
        Admin,

        [Description( "background-search" )]
        Search,

        [Description( "background-full-screen" )]
        FullScreen,

    }
}
