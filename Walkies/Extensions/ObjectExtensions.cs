using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Walkies
{
    public static class ObjectExtensions
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Gets the first attribute of the specified type from the object
        /// </summary>
        /// <returns></returns>
        public static T GetAttribute<T>( this object @object ) where T : Attribute
        {
            var attribute = @object.GetType().GetCustomAttributes( typeof( T ), true ).FirstOrDefault();
            return attribute as T;
        }

        ///// <summary>
        ///// Gets all attributes of the specified type from the object
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<T> GetAttributes<T>( this object @object ) where T : Attribute
        //{
        //    var attribute = @object.GetType().GetCustomAttributes( typeof( T ), true );
        //    return attribute as IEnumerable<T>;
        //}

        /// <summary>
        /// Gets the first property on the object that has an attribute of the specified type
        /// </summary>
        /// <returns></returns>
        public static PropertyInfo GetPropertyWithAttribute<T>( this object @object ) where T : Attribute
        {
            if( @object == null ) return null;
            var type = @object.GetType();
            return type.GetProperties().FirstOrDefault( p => p.GetCustomAttributes( typeof( T ), true ).Count() > 0 );
        }

        /// <summary>
        /// Gets the first property on the object that has an attribute of the specified type
        /// </summary>
        /// <returns></returns>
        public static PropertyInfo GetPropertyWithAttribute<T>( this object @object, Type objectType ) where T : Attribute
        {
            return objectType.GetProperties().FirstOrDefault( p => p.GetCustomAttributes( typeof( T ), true ).Count() > 0 );
        }

        /// <summary>
        /// Gets the value of the first property on the object that has an attribute of the specified type, or the default
        /// value of none is found
        /// </summary>
        /// <returns></returns>
        public static dynamic GetValueOfPropertyWithAttribute<T>( this object @object, dynamic defaultValue = null ) where T : Attribute
        {
            var type = @object.GetType();
            var property = type.GetProperties().FirstOrDefault( p => p.GetCustomAttributes( typeof( T ), true ).Count() > 0 );
            if( property == null ) return defaultValue;
            return property.GetValue( @object );
        }

        /// <summary>
        /// Gets the value of the first property on the object that has an attribute of the specified type, or the default
        /// value of none is found
        /// </summary>
        /// <returns></returns>
        public static dynamic GetValueOfPropertyWithAttribute<T>( this object @object, Type objectType, dynamic defaultValue = null ) where T : Attribute
        {
            var property = objectType.GetProperties().FirstOrDefault( p => p.GetCustomAttributes( typeof( T ), true ).Count() > 0 );
            if( property == null ) return defaultValue;
            return property.GetValue( @object );
        }

        /// <summary>
        /// Gets all properties on the specified object that have an attribute of the specified type
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>( this Type type ) where T : Attribute
        {
            return type.GetProperties().Where( p => p.GetCustomAttributes( typeof( T ), true ).Count() > 0 );
        }

        /// <summary>
        /// Gets all properties on the specified object that have an attribute of the specified type
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>( this object @object ) where T : Attribute
        {
            var type = @object.GetType();
            return type.GetProperties().Where( p => p.GetCustomAttributes( typeof( T ), true ).Count() > 0 );
        }

        /// <summary>
        /// Gets all properties on the specified object that have an attribute of the specified type
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>( this object @object, Type objectType ) where T : Attribute
        {
            return objectType.GetProperties().Where( p => p.GetCustomAttributes( typeof( T ), true ).Count() > 0 );
        }

        /// <summary>
        /// Serializes the object to a JSON string with optional formatting
        /// </summary>
        /// <param name="object"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string ToJsonString( this object @object, Formatting formatting = Formatting.None )
        {
            return JsonConvert.SerializeObject( @object, formatting );
        }

        /// <summary>
        /// Serializes the object to a JSON string with optional formatting
        /// </summary>
        /// <param name="object"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string ToJsonString( this object @object, JsonSerializerSettings settings )
        {
            return JsonConvert.SerializeObject( @object, settings );
        }

        /// <summary>
        /// Serializes the object to a JSON string with optional formatting
        /// </summary>
        /// <param name="object"></param>
        /// <param name="converters"></param>
        /// <returns></returns>
        public static string ToJsonString( this object @object, params JsonConverter[] converters )
        {
            return JsonConvert.SerializeObject( @object, converters );
        }

        /// <summary>
        /// Serializes the object to a JSON string with optional formatting
        /// </summary>
        /// <param name="object"></param>
        /// <param name="formatting"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ToJsonString( this object @object, Formatting formatting, JsonSerializerSettings settings )
        {
            return JsonConvert.SerializeObject( @object, formatting, settings );
        }

        /// <summary>
        /// Serializes the object to a JSON string with optional formatting
        /// </summary>
        /// <param name="object"></param>
        /// <param name="formatting"></param>
        /// <param name="converters"></param>
        /// <returns></returns>
        public static string ToJsonString( this object @object, Formatting formatting, params JsonConverter[] converters )
        {
            return JsonConvert.SerializeObject( @object, formatting, converters );
        }

        /// <summary>
        /// Dumps the object and its properties to the debug window
        /// </summary>
        /// <param name="object"></param>
        public static void ToDebugWindow( this object @object )
        {
            if( @object == null )
            {
                Debug.Print( "null" );
                return;
            }

            Debug.Print( @object.ToString() );
            foreach( PropertyDescriptor descriptor in TypeDescriptor.GetProperties( @object ) )
            {
                Debug.Print( "- {0}={1}", descriptor.Name, descriptor.GetValue( @object ) );
            }
        }

        /// <summary>
        /// Rounds all the decimal values on a particular class to the specified number of decimal places.
        /// </summary>
        public static void RoundAllDecimals( this object @object, int decimalPlaces )
        {
            Type type = @object.GetType();

            List<PropertyInfo> decimalProperties = type.GetProperties().Where( p => p.SetMethod != null && p.GetMethod != null && p.PropertyType == typeof( decimal ) ).ToList();

            foreach ( PropertyInfo property in decimalProperties )
            {
                property.SetValue( @object, Math.Round( (decimal)property.GetValue( @object ), decimalPlaces ) );
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
