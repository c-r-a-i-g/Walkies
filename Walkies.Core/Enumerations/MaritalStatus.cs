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
    public enum MaritalStatus
    {

        /// <summary>
        /// Married
        /// </summary>
        Married = 1,

        /// <summary>
        /// Single
        /// </summary>
        Single = 2

    }
}
