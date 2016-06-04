using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Interfaces
{
    public interface IIntegrationHubTemplateDefinition
    {

        string Key { get; set; }
        string Path { get; set; }
        string Title { get; set; }
        string Type { get; set; }
        string Script { get; set; }
        List<string> Sources { get; set; }
    
    }
}
