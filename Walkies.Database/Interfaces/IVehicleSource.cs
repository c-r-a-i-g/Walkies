using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Database.Interfaces
{
    public interface IVehicleSource
    {
        
        List<KeyValuePair<string, string>> GetMakesList();
        
        List<KeyValuePair<string, string>> GetFamilyList( string make );
        
        List<int> GetYearList( string make, string family );

        List<KeyValuePair<string, string>> GetCodeList( string make, string family, int? year );

        string GetImage( string code );

    }
}
