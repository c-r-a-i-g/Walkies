using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public static class DateTimeExtensions
    {

        // --------------------------------------------------------------------------------------------------------

        #region Class Members

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// Takes a UTC date and using NOW as a reference, returns a friendly string that describes the time difference
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Humanize( this DateTime? date )
        {
            if( date.HasValue == false ) return "N/A";
            return date.Value.Humanize();
        }

        /// <summary>
        /// Takes a date and using NOW as a reference, returns a friendly string that describes the time difference
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Humanize( this DateTime date )
        {
            var timespan = date - DateTime.Now;
            return timespan.HumanizeFromNow();
        }

        /// <summary>
        /// Gets the week of year for the specified date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int WeekOfYear( this DateTime dateTime )
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear( dateTime, dfi.CalendarWeekRule, dfi.FirstDayOfWeek );
        }

        /// <summary>
        /// Gets the start of month date time for specified date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime StartOfMonth( this DateTime dateTime )
        {
            return new DateTime( dateTime.Year, dateTime.Month, 1 );
        }

        /// <summary>
        /// Gets the end of month date time for specified date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime EndOfMonth( this DateTime dateTime )
        {
            return dateTime.StartOfMonth().AddMonths( 1 ).AddSeconds( -1 );
        }

        /// <summary>
        /// Takes a date and using NOW as a reference, returns a simplified string that describes the time difference
        /// using Yesterday, tomnorrow, etc.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Simplify( this DateTime date )
        {

            TimeSpan difference = DateTime.UtcNow - date;

            if( difference.TotalDays < 1 && difference.TotalDays > -1 )
            {
                return date.ToString( "hh:mm tt" ).ToLower();
            }

            else if( difference.TotalDays == 1 )
            {
                return "yesterday";
            }

            else if( difference.TotalDays == -1 )
            {
                return "tomorrow";
            }

            else
            {
                return date.ToString( "dd-MM-yyyy HH:mm" );
            }

        }

        /// <summary>
        /// Takes a date and using NOW as a reference, returns a simplified string that describes the difference
        /// using Yesterday, tomnorrow, etc., returning only the date component
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string SimplifyDateOnly( this DateTime date, string format = "dd-MM-yyyy" )
        {

            if( DateTime.Now.Date == date.Date )
            {
                return "Today";
            }

            TimeSpan difference = DateTime.UtcNow - date;

            if( difference.TotalDays < 2 && difference.TotalDays > 0 )
            {
                return "Yesterday";
            }

            else if( difference.TotalDays > -2 && difference.TotalDays < 0 )
            {
                return "Tomorrow";
            }

            else
            {
                return date.ToString( format );
            }

        }

        #endregion

        // --------------------------------------------------------------------------------------------------------

        #region Private Methods

        #endregion

        // --------------------------------------------------------------------------------------------------------

    }
}
