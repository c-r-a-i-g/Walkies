using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public class DatePickerOptions
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public DatePickerOptions()
        {
            this.ShowDropdowns = false;
            this.Format = "dd/MM/yyyy";
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// Creates a date picker with a min date of today and an optional max date
        /// </summary>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static DatePickerOptions FromToday( DateTime? maxDate = null, bool showDropDowns = false, string format = "dd/MM/yyyy" )
        {
            var result = new DatePickerOptions();
            result.MinDate = DateTime.Now;
            result.MaxDate = maxDate;
            result.ShowDropdowns = showDropDowns;
            result.Format = format;
            return result;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool ShowDropdowns { get; set; }
        public string Format { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public Dictionary<string, object> Attributes
        {
            get
            {

                var attributes = new Dictionary<string,object>();

                attributes.Add( "data-single-date-picker", "true" );
                attributes.Add( "data-format", this.Format.ToUpper() ); // To upper because the datepicker uses moment.js, which has a different format string
                attributes.Add( "data-show-dropdowns", this.ShowDropdowns.ToString().ToLower() );

                if( this.MinDate.HasValue ) attributes.Add( "data-min-date", this.MinDate.Value.ToString( this.Format ) );
                if( this.MaxDate.HasValue ) attributes.Add( "data-max-date", this.MaxDate.Value.ToString( this.Format ) );

                return attributes;

            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
