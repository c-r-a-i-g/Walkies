using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Interfaces
{
    public interface ICacheExpiryHandler
    {
        void CacheExpired();
    }
}
