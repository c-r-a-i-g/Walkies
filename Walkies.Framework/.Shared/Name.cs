using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Shared
{
    public class Name
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public Name( string nameString )
        {

            var parts = nameString.Trim().Split( ' ' );

            if( parts.Length == 1 )
            {
                this.FirstName = parts[ 0 ].Trim();
            }

            else if( parts.Length > 1 )
            {
                this.FirstName = parts[ 0 ].Trim();
                this.LastName = parts[ parts.Length - 1 ].Trim();

                this.OtherNames = "";

                for( int i = 1; i < parts.Length - 1; i += 1 )
                {
                    this.OtherNames += parts[ i ].Trim() + ' ';
                }

                this.OtherNames = this.OtherNames.Trim();
            }

            this.Normalise();

        }

        public Name( string firstName, string lastName, string otherNames = "" )
        {
            this.FirstName = ( firstName ?? "" ).Trim();
            this.LastName = ( lastName ?? "" ).Trim();
            this.OtherNames = ( otherNames ?? "" ).Trim();
            this.Normalise();
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

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Normalises the name
        /// </summary>
        private void Normalise()
        {

            if( string.IsNullOrEmpty( this.FirstName ) == false && this.FirstName.Length > 1 )
            {
                this.FirstName = char.ToUpper( this.FirstName[ 0 ] ) + this.FirstName.Substring( 1 ).ToLower();
            }

            if( string.IsNullOrEmpty( this.LastName ) == false && this.LastName.Length > 1 )
            {
                this.LastName = char.ToUpper( this.LastName[ 0 ] ) + this.LastName.Substring( 1 ).ToLower();
            }

            if( string.IsNullOrEmpty( this.OtherNames ) ) return;

            var parts = this.OtherNames.Trim().Split( ' ' );
            var otherNames = "";
            for( int i = 0; i < parts.Length; i += 1 )
            {
                var part = parts[ i ];
                if( part.Trim().Length == 0 ) continue;

                if( part.Length > 1 )
                {
                    otherNames += char.ToUpper( part[ 0 ] ) + part.Substring( 1 ).ToLower() + " ";
                }

                else
                {
                    otherNames += char.ToUpper( part[ 0 ] ) + " ";
                }

            }

            this.OtherNames = otherNames.Trim();

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public string FullName
        {
            get
            {
                return string.Format( "{0} {1} {2}", this.FirstName, this.OtherNames, this.LastName )
                             .Replace( "  ", " " );
            }
        }

        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty( this.FirstName ) == false 
                    && string.IsNullOrEmpty( this.LastName ) == false;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
