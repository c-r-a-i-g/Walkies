using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum Country
    {
        /// <summary>
        /// Australia
        /// </summary>
        AUS = 1
    }
}
