using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Interfaces
{
    public interface IEditorAttributes
    {

        string EntityName { get; }
        string EditorPath { get; set; }
        bool CanCopyRecords { get; set; }
        bool HasCustomOptions { get; set; }
    }
}
