using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Converts a boolean value to a string using the desired true/false labels.
        /// </summary>
        public static string ToString( this bool flag, string trueLabel, string falseLabel )
        {
            return ( flag == true ? trueLabel : falseLabel );
        }
    }
}
