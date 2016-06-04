using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Converts a decimal value into a time string.
        /// </summary>
        public static string ToFriendlyTime( this decimal totalSeconds )
        {
            return ( (int)Math.Round( totalSeconds, 0 ) ).ToFriendlyTime();
        }

        /// <summary>
        /// Converts a decimal value into a time string of the form: ##:##:##
        /// </summary>
        public static string ToDigitTime( this decimal totalSeconds )
        {
            return ( (int)totalSeconds ).ToDigitTime();
        }

        /// <summary>
        /// Converts the amount into a currency string
        /// </summary>
        public static string ToCurrency( this decimal amount, int decimalPlaces = 0 )
        {
            return string.Format( "{0:C0}", Math.Round( amount, decimalPlaces ) ).Replace( "(", "-" ).Replace( ")", "" );
        }

        /// <summary>
        /// Converts the amount into a percentage (applies rounding)
        /// </summary>
        public static string ToPercentage( this decimal amount )
        {
            return string.Format( "{0}%", Math.Round( amount ) );
        }

        /// <summary>
        /// Turns the decimal value into a friendly currency format.
        /// </summary>
        public static string ToFriendlyCurrency( this decimal amount )
        {
            string currency;
            if ( amount >= 1000000 )
            {
                currency = amount.ToString( "#,,.0M" );
            }
            else if ( amount >= 100000 )
            {
                currency = amount.ToString( "#,.#k" );
            }
            else if ( amount >= 1000 )
            {
                currency = amount.ToString( "#,.0k" );
            }
            else
            {
                currency = amount.ToString();
            }

            return string.Format( "${0}", currency );
        }
    }
}
