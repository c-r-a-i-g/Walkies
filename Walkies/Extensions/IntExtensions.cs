using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walkies
{
    public static class IntExtensions
    {
        /// <summary>
        /// Checks to see if the provided enumerated value is equal to the current integer value.
        /// </summary>
        /// <returns>False if the value provided is not an enum, true if the value is successfully converted to an integer and equals the current integer value</returns>
        public static bool IsEqualTo<T>( this int intValue, T enumValue ) where T : IConvertible
        {
            if ( typeof( T ).IsEnum )
            {
                return ( (int)Convert.ChangeType( enumValue, typeof( int ) ) == intValue );
            }

            return false;
        }

        /// <summary>
        /// Checks to see if the provided enumerated value is not equal to the current integer value.
        /// </summary>
        /// <returns>False if the value provided is not an enum, true if the value is successfully converted to an integer and does not equal the current integer value</returns>
        public static bool IsNotEqualTo<T>( this int intValue, T enumValue ) where T : IConvertible
        {
            if ( typeof( T ).IsEnum )
            {
                return ( (int)Convert.ChangeType( enumValue, typeof( int ) ) != intValue );
            }

            return false;
        }

        /// <summary>
        /// Converts an integer value into a reader-friendly time string.
        /// </summary>
        /// <param name="totalSeconds"></param>
        /// <returns></returns>
        public static string ToFriendlyTime( this int totalSeconds )
        {
            return TimeSpan.FromSeconds( totalSeconds ).Humanize();
        }

        /// <summary>
        /// Converts in integer value into a time string of the form: d.hh:mm:ss
        /// </summary>
        public static string ToDigitTime( this int totalSeconds )
        {

            TimeSpan timeSpan = TimeSpan.FromSeconds( totalSeconds );

            int hours = timeSpan.Hours + timeSpan.Days * 24;
            string hourString = ( hours < 10 ? "0" : "" ) + hours.ToString();

            return ( hourString + ":" + timeSpan.ToString( "mm\\:ss" ) );

        }

        /// <summary>
        /// Converts the time from seconds into a more readible format, along with a suffix.
        /// </summary>
        public static string ToTime( this int totalSeconds, bool shortFormat, out string suffix )
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds( totalSeconds );

            if ( timeSpan.Hours > 1 )
            {
                suffix = ( shortFormat ? "hrs" : "hours" );

                if ( timeSpan.Hours > 9 )
                {
                    return timeSpan.Hours.ToString();
                }

                return string.Format( "{0:N1}", timeSpan.Minutes / 60m );
            }

            if ( timeSpan.TotalMinutes > 1 )
            {
                suffix = ( shortFormat ? "mins" : "minutes" );
                return string.Format( "{0:N0}", timeSpan.TotalMinutes );
            }

            if ( timeSpan.Minutes == 1 )
            {
                suffix = ( shortFormat ? "min" : "minute" );
                return "1";
            }

            if ( timeSpan.Seconds == 1 )
            {
                suffix = ( shortFormat ? "sec" : "second" );
                return "1";
            }

            suffix = ( shortFormat ? "secs" : "seconds" );
            return timeSpan.Seconds.ToString();
        }

        /// <summary>
        /// Converts the time from seconds into hours and minutes.
        /// </summary>
        /// <returns>Return hours minutes if hours are greater than 0, return only minutes if minutes are greater than 0 otherwise returns "-" symbol </returns>
        public static string ToHourMinute( this int seconds, string hoursSuffix = "h", string minutesSuffix = "m", string defaultIfZero = "-" )
        {
            TimeSpan timespan = TimeSpan.FromSeconds( seconds );
            var days = (int)timespan.Days;
            var hours = (int)timespan.Hours;
            var minutes = (int)timespan.Minutes;

            int totalHours = hours;

            if ( days > 0 )
            {
                totalHours += days * 24;
            }

            if ( hours > 0 )
            {
                return string.Format( "{0}{1} {2}{3}", totalHours, hoursSuffix, minutes, minutesSuffix );
            }
            else if ( minutes > 0 )
            {
                return string.Format( "{0}{1}", minutes, minutesSuffix );
            }
            else
            {
                return defaultIfZero;
            }
        }

        /// <summary>
        /// Converts the time from seconds into hours and minutes.
        /// </summary>
        /// <returns>Return hours minutes seconds if hours are greater than 0, return only minutes and seconds if minutes are greater than 0, returns seconds if seconds are greater than 0 otherwise returns default "-" symbol </returns>
        public static string ToHourMinuteSecond( this int seconds, string hoursSuffix = "h", string minutesSuffix = "m", string secondsSuffix = "s", string defaultIfZero = "-" )
        {
            TimeSpan timespan = TimeSpan.FromSeconds( seconds );
            var days = (int)timespan.Days;
            var hours = (int)timespan.Hours;
            var minutes = (int)timespan.Minutes;
            var secondsCount = (int)timespan.Seconds;

            int totalHours = hours;

            if ( days > 0 )
            {
                totalHours += days * 24;
            }

            if ( hours > 0 )
            {
                return string.Format( "{0}{1} {2}{3} {4}{5}", totalHours, hoursSuffix, minutes, minutesSuffix, secondsCount, secondsSuffix );
            }
            else if ( minutes > 0 )
            {
                return string.Format( "{0}{1} {2}{3}", minutes, minutesSuffix, secondsCount, secondsSuffix );
            }
            else if ( secondsCount > 0 )
            {
                return string.Format( "{0}{1}", secondsCount, secondsSuffix );
            }
            else
            {
                return defaultIfZero;
            }
        }

        /// <summary>
        /// Converts the amount into a currency string
        /// </summary>
        public static string ToCurrency( this int amount )
        {
            return string.Format( "{0:C0}", amount ).Replace( "(", "-" ).Replace( ")", "" );
        }

        /// <summary>
        /// Converts the amount into a percentage
        /// </summary>
        public static string ToPercentage( this int amount )
        {
            return string.Format( "{0}%", amount );
        }
    }
}