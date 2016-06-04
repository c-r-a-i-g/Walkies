using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Walkies.Interfaces;
using Newtonsoft.Json;

namespace Walkies
{
    public static class Enums
    {
        public static Dictionary<string, int> LoadNames<T>()
        {
            Type _type = typeof( T );
            FieldInfo[] fields = _type.GetFields();
            Dictionary<string, int> retVal = new Dictionary<string, int>();
            Type _descriptionType = typeof( DescriptionAttribute );
            if( _type.IsEnum )
            {
                foreach( var field in fields )
                {
                    if( field.Name != "value__" )
                    {
                        var descAttr = Attribute.GetCustomAttribute( field, _descriptionType ) as DescriptionAttribute;
                        if( descAttr != null && !String.IsNullOrEmpty( descAttr.Description ) )
                            retVal.Add( descAttr.Description, (int)field.GetRawConstantValue() );
                        else
                            retVal.Add( field.Name, (int)field.GetRawConstantValue() );
                    }
                }
            }
            else
                throw new Exception( "Is not Enum." );
            return retVal;
        }

        public static IList<T> LoadAllFields<T>()
        {
            Type _type = typeof( T );
            IList<T> retVal = new List<T>();
            Array vals = Enum.GetValues( _type );
            if( _type.IsEnum )
            {
                foreach( T val in vals )
                {
                    retVal.Add( val );
                }
            }
            return retVal;
        }

        /// <summary>
        /// Loads all field values as integers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<int> LoadAllFieldsAsInt<T>()
        {
            return LoadAllFields<T>().Select( s => Convert.ToInt32( s.ToString() ) ).ToList();
        }

        public static string LoadName<T>( int _enum )
        {
            Type _type = typeof( T );
            if( !_type.IsEnum )
                throw new Exception( "Is not Enum." );

            return ( (Enum)Enum.Parse( typeof( T ), _enum.ToString() ) ).LoadName();
        }

        public static string LoadName( this Enum _enum )
        {
            if( _enum == null ) return "";
            Type _type = _enum.GetType();
            string retVal = _enum.ToString();

            try
            {
                var descAttr = Attribute.GetCustomAttribute( _type.GetField( retVal ), typeof( DescriptionAttribute ) ) as DescriptionAttribute;
                if( descAttr != null )
                    retVal = descAttr.Description;
                return retVal;
            }

            catch
            {
                return retVal;
            }
        }

        /// <summary>
        /// Gets the value of the attribute identified by T.  T must implement IAttributeValue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetAttributeValue<T>( this Enum @enum ) where T : IAttributeValue
        {
            var @type = @enum.GetType();
            var result = @enum.ToString();

            try
            {
                var attribute = Attribute.GetCustomAttribute( @type.GetField( result ), typeof( T ) ) as IAttributeValue;
                if( attribute != null )
                {
                    return attribute.Value;
                }
                return "";
            }

            catch
            {
                return "";
            }

        }

        /// <summary>
        /// Gets the value of the ClassAttribute on the enum, if it has one
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetClass( this Enum @enum )
        {
            return @enum.GetAttributeValue<ClassAttribute>();
        }

        public static T GetEnumValue<T>( int _enum )
        {
            return (T)Enum.Parse( typeof( T ), _enum.ToString() );
        }

        public static T GetEnumValue<T>( string _enum )
        {
            if( !string.IsNullOrEmpty( _enum ) )
                return (T)Enum.Parse( typeof( T ), _enum );
            return default( T );
        }

        public static SelectList ToSelectList<TEnum>( this TEnum enumObj, string Id = "Id", string Name = "Name" )
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            int value = Convert.ToInt32( enumObj );
            var values = from TEnum e in Enum.GetValues( typeof( TEnum ) ) select new { Id = Convert.ToInt32( Enum.Parse( typeof( TEnum ), e.ToString() ) ), Name = e.ToString(), Description = ( e as Enum ).GetDescription() };
            return new SelectList( values, Id, Name, Convert.ToInt32( enumObj ) );
        }

        public static SelectList ToSelectList<TEnum>( string Id = "Id", string Name = "Name" ) where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues( typeof( TEnum ) ) select new 
            { 
                Id = Convert.ToInt32( Enum.Parse( typeof( TEnum ), e.ToString() ) ),
                Name = ( e as Enum ).GetDescription()
            };
            return new SelectList( values, Id, Name );
        }

        public static string ToJson<TEnum>( this TEnum enumObj )
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            Dictionary<string, int> items = Enum.GetValues( typeof( TEnum ) ).Cast<int>().Select( e => new { Name = Enum.GetName( typeof( TEnum ), e ), Value = e } ).ToDictionary( d => d.Name, d => d.Value );
            return JsonConvert.SerializeObject( items );
        }

        public static string GetDescription( this Enum _enum )
        {
            return _enum.LoadName();
        }

    }
}
