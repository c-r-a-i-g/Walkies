using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum Title
    {

        /// <summary>
        /// Mr
        /// </summary>
        Mr = 1,

        /// <summary>
        /// Mrs
        /// </summary>
        Mrs = 2,

        /// <summary>
        /// Ms
        /// </summary>
        Ms = 3
    }
}
