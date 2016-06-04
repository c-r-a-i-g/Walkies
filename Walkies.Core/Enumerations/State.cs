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
    public enum State
    {

        /// <summary>
        /// New South Wales
        /// </summary>
        NSW = 1,

        /// <summary>
        /// Victoria
        /// </summary>
        VIC = 2,

        /// <summary>
        /// Queensland
        /// </summary>
        QLD = 3,

        /// <summary>
        /// South Australia
        /// </summary>
        SA = 4,

        /// <summary>
        /// Western Australia
        /// </summary>
        WA = 5,

        /// <summary>
        /// Australian Capital Territory
        /// </summary>
        ACT = 6,

        /// <summary>
        /// Tasmania
        /// </summary>
        TAS = 7,

        /// <summary>
        /// Northern Territory
        /// </summary>
        NT = 8

    }
}
