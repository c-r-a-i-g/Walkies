using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Owin;

namespace Walkies.Framework.Initialisation
{
    public interface IAppStartup : IDisposable
    {
        void Execute( IAppBuilder app );
    }
}
