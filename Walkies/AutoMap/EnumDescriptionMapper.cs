using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Walkies.AutoMapper
{
    public class EnumDescriptionMapper : IObjectMapper
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public bool IsMatch( ResolutionContext context )
        {

            if( context.SourceType.IsEnum && context.DestinationType == typeof(string) ) return true;
            if( context.SourceType == typeof( string ) && context.DestinationType.IsEnum ) return true;

            if( this.IsNullableEnum( context.SourceType ) && context.DestinationType == typeof( string ) ) return true;
            if( context.SourceType == typeof( string ) && this.IsNullableEnum( context.DestinationType ) ) return true;

            return false;

        }

        public object Map( ResolutionContext context, IMappingEngineRunner mapper )
        {

            if( context.SourceType.IsEnum && context.DestinationType == typeof( string ) )
            {
                return this.ConvertEnumToString( context.SourceType, context.SourceValue );
            }

            return this.ConvertStringToEnum( context.SourceValue.ToString(), context.DestinationType );

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

        private bool IsNullableEnum( Type type )
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof( Nullable<> )
                                      && type.GetGenericArguments()[ 0 ].IsEnum;
        }

        private Type GetGenericType( Type nullableType )
        {
            return nullableType.GenericTypeArguments.First();
        }

        private string ConvertEnumToString( Type enumType, object sourceValue )
        {

            var isNullable = this.IsNullableEnum( enumType );
            if( isNullable && sourceValue == null ) return "";

            Enum instance = (Enum)Enum.Parse( enumType, sourceValue.ToString() );
            var description = instance.GetDescription();

            if( string.IsNullOrWhiteSpace( description ) == false )
            {
                return description;
            }

            return instance.ToString();

        }

        private object ConvertStringToEnum( string sourceValue, Type enumType )
        {

            var isNullable = this.IsNullableEnum( enumType );
            if( isNullable && sourceValue == null ) return null;

            var type = isNullable ? this.GetGenericType( enumType ) : enumType;

            var values = Enum.GetValues( type );
            foreach( var value in values )
            {
                if( ( (Enum)value ).ToString() == sourceValue ) return value;
                if( ( (Enum)value ).GetDescription() == sourceValue ) return value;
            }
            throw new Exception( string.Format( "Unable to convert string '{0}' to enum type {1}.  The enum does not have a value or a value description attribute with the specified string value.", sourceValue, enumType.ToString() ) );
        }

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
