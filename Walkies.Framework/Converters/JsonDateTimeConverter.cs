using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Walkies.Framework.Converters
{
    public class JsonDateTimeConverter : JsonConverter
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public override bool CanConvert( Type objectType )
        {
            return ( objectType == typeof( DateTime ) );
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            if ( existingValue == null )
            {
                return null;
            }

            string dateString = existingValue.ToString();
            DateTime date;

            if ( DateTime.TryParse( dateString, out date ) )
            {
                return date;
            }

            return null;
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            if ( value == null )
            {
                writer.WriteUndefined();
            }
            else if ( value is DateTime )
            {
                writer.WriteValue( ( (DateTime)value ).ToString( "dd/MM/yyyy" ) );
            }
            else
            {
                writer.WriteUndefined();
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
