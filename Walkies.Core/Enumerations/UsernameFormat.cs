using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    public enum UsernameFormat
    {
        [Description( "Email Address" )]
        EmailAddress = 1,

        [Description( "First-name Last-name" )]
        FirstNameAndLastName = 2,

        [Description( "First-initial Last-name" )]
        FirstInitialAndLastName = 3
    }
}
