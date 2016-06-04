using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    public enum CacheFindState
    {
        FoundInCache = 0,
        NotFoundInCache = 1,
        RetrievedFromDatabase = 2
    }
}
