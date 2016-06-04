using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    public enum Sidebar
    {

        [Description( "" )]
        None,

        [Description( "_Sidebars/Admin.Sidebar" )]
        Admin,

        [Description( "_Sidebars/Reports.Sidebar" )]
        Reports,

        [Description( "_Sidebars/Database.Sidebar" )]
        Database,

        [Description( "_Sidebars/Deal.Sidebar" )]
        Deal,

    }
}
