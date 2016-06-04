using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Walkies.Framework.Initialisation
{
    public interface IPostStartup : IDisposable
    {

        void Execute( HttpApplication context );

    }
}
