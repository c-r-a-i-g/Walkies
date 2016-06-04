using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.ViewRendering
{
    public interface IModelRenderService
    {

        string Render( ModelRenderBase model );
        T Render<T>( ModelRenderBase<T> model ) where T : IModelRenderResult, new();

    }

}
