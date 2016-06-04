using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;

namespace Walkies
{
    public static class StringEx
    {

        /// <summary>
        /// Replaces all instances of old value with new value, using the specified string comparison option
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static string ReplaceString( this string str, string oldValue, string newValue, StringComparison comparison )
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf( oldValue, comparison );
            while( index != -1 )
            {
                sb.Append( str.Substring( previousIndex, index - previousIndex ) );
                sb.Append( newValue );
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf( oldValue, index, comparison );
            }
            sb.Append( str.Substring( previousIndex ) );

            return sb.ToString();
        }

        /// <summary>
        /// Gets all characters from the specified index
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string From( this string str, int n )
        {
            if( str == null ) return null;

            var len = str.Length;

            if( n >= len ) return "";
            if( n == 0 || -n >= len ) return str;

            return str.Substring( ( len + n ) % len, ( len - n ) % len );
        }

        /// <summary>
        /// Gets all characters up to the specified index
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string To( this string str, int n )
        {
            if( str == null ) return null;

            var len = str.Length;

            if( n == 0 || -n >= len ) return "";
            if( n >= len ) return str;

            return str.Substring( 0, ( len + n ) % len );
        }

        /// <summary>
        /// Gets all characters up to the specified character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string ToFirst( this string str, char @char )
        {

            if( str == null ) return null;
            if( str.Contains( @char ) == false ) return str;

            return str.Substring( 0, str.IndexOf( @char ) );

        }

        /// <summary>
        /// Fetch the left most characters from a string
        /// </summary>
        public static string Left( this string value, int length )
        {

            if ( string.IsNullOrEmpty( value ) ) return ( string.Empty );
            if ( length < 0 ) return string.Empty;

            if ( value.Length > length )
            {
                value = value.Substring( 0, length );
            }

            return value;

        }

        /// <summary>
        /// Fetch the right most characters from a string
        /// </summary>
        public static string Right( this string value, int length )
        {

            if ( string.IsNullOrEmpty( value ) ) return ( string.Empty );
            if ( length < 0 ) return string.Empty;

            if ( value.Length > length )
            {
                value = value.Substring( value.Length - length, length );
            }

            return value;
        }

        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        public static string ToTitleCase( this string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return string.Empty;
            if ( value != null && char.IsLower( value[ 0 ] ) )
            {
                value = value[ 0 ].ToString().ToUpper() + value.Substring( 1 );
            }

            return value;
        }

        /// <summary>
        /// Converts a string to lower case, then converts the first letter to upper case.
        /// </summary>
        public static string ToForcedTitleCase( this string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return string.Empty;

            value = value.ToLower();

            value = value[ 0 ].ToString().ToUpper() + value.Substring( 1 );

            return value;
        }

        /// <summary>
        /// Converts a string to a css-class friendly name
        /// </summary>
        public static string ToCSSClass( this string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return string.Empty;

            value = char.ToLower( value[ 0 ] ) + value.Substring( 1 );
            value = value.Substring( 0, value.Length - 1 ) + char.ToLower( value[ value.Length - 1 ] );
            value = value.Replace( ' ', '-' );

            for ( int i = 1 ; i < value.Length - 1 ; i++ )
            {
                if ( char.IsUpper( value[ i ] ) )
                {
                    value = value.Substring( 0, i ) + char.ToLower( value[ i ] ) + value.Substring( i + 1 );
                    if ( value[ i - 1 ] != '-' )
                    {
                        value = value.Insert( i, "-" );
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Converts a string to camel case (useful for Json)
        /// </summary>
        public static string ToCamelCase( this string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return string.Empty;

            string[] values = value.Split( ' ' );
            string result = "";

            for ( int i = 0 ; i < values.Length ; i++ )
            {
                var current = values[ i ];
                
                if ( i == 0 )
                {
                    result = char.ToLower( current[ 0 ] ) + current.Substring( 1 );
                }
                else
                {
                    result += char.ToUpper( current[ 0 ] ) + current.Substring( 1 );
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a string to a friendlier version. Useful for converting types to a user readable format.
        /// </summary>
        /// <remarks>
        /// This method was based off the ToCSSClass method.
        /// </remarks>
        public static string ToFriendlyText( this string value )
        {
            if ( string.IsNullOrEmpty( value ) )
                return string.Empty;

            value = value.Substring( 0, value.Length - 1 ) + char.ToLower( value[ value.Length - 1 ] );

            for ( int i = 1 ; i < value.Length - 1 ; i++ )
            {
                if ( char.IsUpper( value[ i ] ) )
                {
                    if ( value[ i - 1 ] != ' ' )
                    {
                        value = value.Insert( i, " " );
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Formats the specified string as a phone number. If the input string is not a number, then the input string is returned.
        /// </summary>
        /// <param name="number">The number to convert</param>
        /// <returns>The string in an easy to read number format</returns>
        public static string ToPhoneNumber( this string number )
        {

            Int64 result;

            number = number.Replace( " ", "" ); // Remove all white space

            if ( Int64.TryParse( number, out result ) == false ) // If the value cannot be converted to an integer, then it mustn't be a number, so return it as is
            {
                return number;
            }

            if ( number.Length == 11 && number.StartsWith( "61" ) ) // International number for Australia
            {
                return result.ToString( "+## ### ### ###" );
            }

            if ( number.Length == 10 )
            {

                if ( number.StartsWith( "04" ) ) // Mobile phone number
                {
                    return result.ToString( "0### ### ###" );
                }

                if ( number.StartsWith( "02" ) || number.StartsWith( "03" ) || number.StartsWith( "07" ) || number.StartsWith( "08" ) ) // Land line with area code
                {
                    return result.ToString( "(0#) ## ### ###" );
                }

            }

            if ( number.Length == 8 ) // Land line
            {

                if ( number.StartsWith( "0" ) )
                {
                    return result.ToString( "0# ### ###" );
                }

                return result.ToString( "## ### ###" );

            }

            // Unknown phone number, so format so that numbers are grouped by pairs
            string formatted = string.Empty;

            if ( number.StartsWith( "0" ) )
            {
                formatted += "0";
            }

            formatted += result.ToString();
            int length = formatted.Length;
            for ( int i = length - 2 ; i > 0 ; i -= 2 )
            {
                formatted = formatted.Insert( i, " " );
            }

            return formatted;

        }

        /// <summary>
        /// Formats the number of seconds into a human readable format. Does not support days or greater.
        /// </summary>
        /// <param name="seconds">The number of seconds to convert into a human readable format</param>
        /// <returns>The number of seconds in a human readable format</returns>
        public static string ToTime( this int seconds )
        {

            var ts = TimeSpan.FromSeconds( seconds );

            int hours = (int)Math.Truncate( ts.TotalHours );

            if ( hours > 0 ) // If there are hours, then format it to not include seconds
            {
                return String.Format( "{0}h {1}m", hours, ts.Minutes );
            }
            else if ( ts.Minutes > 0 )
            {
                return String.Format( "{0}m {1}s", ts.Minutes, ts.Seconds );
            }

            return String.Format( "{0}s", ts.Seconds );

        }

        /// <summary>
        /// Converts the provided string into a percentage string. Returns the value if it cannot be converted into a number.
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <returns>The value as a percentage</returns>
        public static string ToPercentage( this string value )
        {

            decimal valueAsDecimal;

            if ( decimal.TryParse( value, out valueAsDecimal ) )
            {
                return string.Format( "{0}%", (int)Math.Round( valueAsDecimal ) );
            }
            
            return value;
            
        }

        public static string FormatWith( this string format, object source )
        {
            return FormatWith( format, null, source );
        }
        
        public static string FormatWith( this string format, IFormatProvider provider, object source )
        {
            if( format == null )
                throw new ArgumentNullException( "format" );

            Regex r = new Regex( @"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
                          RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase );

            List<object> values = new List<object>();
            string rewrittenFormat = r.Replace( format, delegate( Match m )
            {

                Group startGroup = m.Groups[ "start" ];
                Group propertyGroup = m.Groups[ "property" ];
                Group formatGroup = m.Groups[ "format" ];
                Group endGroup = m.Groups[ "end" ];

                values.Add( ( propertyGroup.Value == "0" )
                  ? source
                  : DataBinder.Eval( source, propertyGroup.Value ) );

                return new string( '{', startGroup.Captures.Count ) + ( values.Count - 1 ) + formatGroup.Value
                  + new string( '}', endGroup.Captures.Count );
            } );

            return string.Format( provider, rewrittenFormat, values.ToArray() );
        }


        /// <summary>
        /// Returns true if the string starts with any of the strings in the compareTo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StartsWithAny( this string value, IEnumerable<string> compareTo )
        {

            foreach( string compare in compareTo )
            {
                if( value.StartsWith( compare ) ) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the string starts with any of the strings in the compareTo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EndsWithAny( this string value, IEnumerable<string> compareTo )
        {

            foreach( string compare in compareTo )
            {
                if( value.EndsWith( compare ) ) return true;
            }

            return false;
        }

        /// <summary>
        /// Replaces all string found in the compareTo enumeration with the replaceWith value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceAny( this string value, IEnumerable<string> compareTo, string replaceWith )
        {

            if( value == null ) return "";

            foreach( string compare in compareTo )
            {
                value = value.Replace( compare, replaceWith );
            }

            return value;
        }

        /// <summary>
        /// Splits a string on the specified character, and removes empty entries
        /// </summary>
        /// <param name="value"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string[] Split( this string value, char splitOn )
        {
            return value.Split( new char[] { splitOn }, StringSplitOptions.RemoveEmptyEntries );
        }

        /// <summary>
        /// Splits a string on the specified character, and removes empty entries
        /// </summary>
        /// <param name="value"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string[] Split( this string value, string splitOn, bool trimResults = false )
        {
            if( trimResults )
            {
                return value.Split( new string[] { splitOn }, StringSplitOptions.RemoveEmptyEntries ).Select( s => s.Trim() ).ToArray();
            }
            return value.Split( new string[] { splitOn }, StringSplitOptions.RemoveEmptyEntries );
        }

        /// <summary>
        /// Returns true if the string is not null and has at least one character length, even if the character is a space
        /// </summary>
        /// <param name="value"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool HasValue( this string value )
        {
            return string.IsNullOrEmpty( value ) == false;
        }

        /// <summary>
        /// Returns string of words splited by title case characters in specified string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SplitTitleCase( this string value )
        {
            if( string.IsNullOrEmpty( value ) ) return value;

            Regex r = new Regex( @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace );
            return r.Replace( value, " " );
        }

    }
}
