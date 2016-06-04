using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Enumerations
{
    public enum NotificationType
    {

        [Description( "alert-success" )]
        Success = 0,

        [Description( "alert-info" )]
        Information = 1,

        [Description( "alert-warning" )]
        Warning = 2,

        [Description( "alert-danger" )]
        Danger = 3

    }

}
