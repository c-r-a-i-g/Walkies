using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public class DropDownOptions
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private const int DEFAULT_LIST_SIZE = 5;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public DropDownOptions()
        {
            this.ListSize = DEFAULT_LIST_SIZE;
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
        public static DropDownOptions DependsOn( string dependsOnId, string dataUrl, string emptyLabel, int listSize = DEFAULT_LIST_SIZE )
        {
            var result = new DropDownOptions();
            result.DependsOnId = dependsOnId;
            result.DataUrl = dataUrl;
            result.EmptyLabel = emptyLabel;
            result.ListSize = listSize;
            return result;
        }

        /// <summary>
        /// Creates a date picker with a min date of today and an optional max date
        /// </summary>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public static DropDownOptions Multi( string title, int collapseAfter, int listSize = DEFAULT_LIST_SIZE )
        {
            var result = new DropDownOptions();
            result.Title = title;
            result.CollapseAfter = collapseAfter;
            result.ListSize = listSize;
            return result;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public int ListSize { get; set; }
        public string DependsOnId { get; set; }
        public string DataUrl { get; set; }
        public string EmptyLabel { get; set; }
        public string Title { get; set; }
        public int CollapseAfter { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public Dictionary<string, object> Attributes
        {
            get
            {

                var attributes = new Dictionary<string,object>();

                attributes.Add( "data-size", this.ListSize );

                if( string.IsNullOrEmpty( this.DependsOnId ) == false ) attributes.Add( "data-depends-on", this.DependsOnId );
                if( string.IsNullOrEmpty( this.DataUrl ) == false ) attributes.Add( "data-data-url", this.DataUrl );
                if( string.IsNullOrEmpty( this.EmptyLabel ) == false ) attributes.Add( "data-empty-label", this.EmptyLabel );
                if( string.IsNullOrEmpty( this.Title ) == false ) attributes.Add( "title", this.Title );
                
                if( this.CollapseAfter > 0 )
                {
                    attributes.Add( "data-selected-text-format", "count>" + this.CollapseAfter );
                }

                return attributes;

            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
