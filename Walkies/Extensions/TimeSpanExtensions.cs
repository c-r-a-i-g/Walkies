using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{

    public static class TimeSpanExtensions
    {

        // --------------------------------------------------------------------------------------------------------

        #region Class Members

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// Takes a TimeSpan and returns a friendly string that describes its duration
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string HumanizeFromNow( this TimeSpan? timespan )
        {
            if( timespan.HasValue == false ) return "N/A";
            return timespan.Value.HumanizeFromNow();
        }

        /// <summary>
        /// Takes a TimeSpan and returns a friendly string that describes how far away it is from now
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string HumanizeFromNow( this TimeSpan timespan, int secondsToNormaliseToNow = 2 )
        {

            var calendar = new GregorianCalendar();
            var lastWeek = calendar.GetWeekOfYear( DateTime.Now + timespan, CalendarWeekRule.FirstDay, DayOfWeek.Monday );
            var thisWeek = calendar.GetWeekOfYear( DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday );
            var week = lastWeek - thisWeek;
            var day = ( DateTime.Now + timespan ).ToString( "dddd" );
            var years = (int)Math.Round( (decimal)timespan.TotalDays / 365 );
            var months = (int)Math.Round( (decimal)timespan.TotalDays / 30 );
            var weeks = (int)Math.Round( (decimal)timespan.Days / 7 );
            var days = (int)Math.Round( timespan.TotalDays );
            var hours = (int)Math.Round( timespan.TotalHours );
            var minutes = (int)Math.Round( timespan.TotalMinutes );
            var seconds = (int)Math.Round( timespan.TotalSeconds );
            var milliseconds = (int)Math.Round( timespan.TotalMilliseconds );

            if( timespan.TotalMilliseconds > 0 )
            {
                if( seconds < secondsToNormaliseToNow ) return "Now";
                else if( seconds < 20 ) return "In a few seconds";
                else if( seconds < 70 ) return "In a minute";
                else if( minutes < 4 ) return "In a few minutes";
                else if( hours < 1 ) return string.Format( "In {0} minutes", minutes );
                else if( hours == 1 && days == 0 ) return "In 1 hour";
                else if( days < 1 ) return string.Format( "In {0} hours", hours );
                else if( days == 1 && weeks == 0 ) return "Tomorrow";
                else if( week == 1 ) return "Next " + day;
                else if( weeks < 1 ) return string.Format( "In {0} days", days );
                else if( weeks == 1 && months == 0 ) return "In 1 week";
                else if( months < 1 ) return string.Format( "In {0} weeks", weeks );
                else if( months == 1 && years == 0 ) return "In 1 month";
                else if( years < 1 ) return string.Format( "In {0} months", months );
                else if( years == 1 ) return "In 1 year";
                else return string.Format( "In {0} years", years );
            }

            if( seconds > -secondsToNormaliseToNow ) return "Now";
            else if( seconds > -20 ) return "A few seconds ago";
            else if( seconds > -70 ) return "A minute ago";
            else if( minutes > -4 ) return "A few minutes ago";
            else if( hours > -1 ) return string.Format( "{0} minutes ago", Math.Abs( minutes ) );
            else if( hours == -1 && days == 0 ) return "1 minute ago";
            else if( days > -1 ) return string.Format( "{0} hours ago", Math.Abs( hours ) );
            else if( days == -1 && weeks == 0 ) return "Yesterday";
            else if( week == -1 ) return "Last " + day;
            else if( weeks > -1 ) return string.Format( "{0} days ago", Math.Abs( days ) );
            else if( weeks == -1 && months == 0 ) return "1 week ago";
            else if( months > -1 ) return string.Format( "{0} weeks ago", Math.Abs( weeks ) );
            else if( months == -1 && years == 0 ) return "1 month ago";
            else if( years > -1 ) return string.Format( "{0} months ago", Math.Abs( months ) );
            else if( years == -1 ) return "1 year ago";
            else return string.Format( "{0} years ago", Math.Abs( years ) );

        }

        /// <summary>
        /// Takes a TimeSpan and returns a friendly string that describes its duration
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Humanize( this TimeSpan timespan )
        {

            var days = (int)timespan.TotalDays;
            var hours = (int)timespan.TotalHours;
            var minutes = (int)timespan.TotalMinutes;
            var seconds = (int)timespan.TotalSeconds;
            var milliseconds = (int)timespan.TotalMilliseconds;

            if( timespan.TotalMilliseconds == 1 )
            {
                return "1 millisecond";
            }

            else if( timespan.TotalMilliseconds == 1000 )
            {
                return "1 second";
            }

            else
            {
                if( seconds < 1 ) return string.Format( "{0} milliseconds", milliseconds );
                else if( minutes < 1 ) return timespan.ToString( @"ss\.fff" ) + " seconds";
                else if( hours < 1 ) return timespan.ToString( @"mm'm 'ss's'" );
                else if( days < 1 ) return timespan.ToString( @"hh'h 'mm'm 'ss's'" );
                else return timespan.ToString( @"dd'd 'hh'h 'mm'm'" );
            }

        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Private Methods

        #endregion

        // --------------------------------------------------------------------------------------------------------

    }
}
