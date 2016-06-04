using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{

    public enum PageHeaderType
    {

        [Description( "default" )]
        Default = 1,

        [Description( "web-api" )]
        WebApi = 2,

        [Description( "anonymous-user")]
        AnonymousUser = 3,

    }

}
