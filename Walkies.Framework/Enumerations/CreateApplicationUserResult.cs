using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Enumerations
{
    public enum CreateApplicationUserResult
    {
        Successful = 0,
        Failed = 1,
        EmailAddressAlreadyExists = 2
    }
}
