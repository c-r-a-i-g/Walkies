using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{

    public enum RoleType
    {

        [Description( "Administrator" )]
        Administrator = 1,

        [Description( "Manager" )]
        Manager = 2,

        [Description( "General User" )]
        GeneralUser = 3,

        [Description( "Sales Person" )]
        SalesPerson = 4,

        [Description( "API User" )]
        ApiUser = 100,

    }

}
