using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Walkies.Serializers
{
    public class XmlBuilder
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Page Actions

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Ajax Actions

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Events

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods
        /// <summary>
        /// Serializes a class into XML
        /// </summary>
        public static string Serialize<T>( T obj ) where T : class
        {
            return Serialize( obj, typeof( T ) );
        }

        /// <summary>
        /// Serializes a class into XML
        /// </summary>
        public static string Serialize( object obj, Type type )
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add( "", "" );

            XmlSerializer serializer = new XmlSerializer( type );
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.OmitXmlDeclaration = true;

            using ( XmlWriter stringWriter = XmlWriter.Create( builder, xmlSettings ) )
            {
                serializer.Serialize( stringWriter, obj, ns );
                return builder.ToString();
            }
        }

        /// <summary>
        /// Deserializes a string into a class.
        /// </summary>
        public static T Deserialize<T>( string xml ) where T : new()
        {
            return ( (T)Deserialize( xml, typeof( T ) ) );
        }

        /// <summary>
        /// Deserializes a string into a class.
        /// </summary>
        public static object Deserialize( string xml, Type type )
        {
            XmlSerializer serializer = new XmlSerializer( type );
            StringReader reader = new StringReader( xml );
            XmlReader xmlReader = XmlReader.Create( reader );

            object result = serializer.Deserialize( xmlReader );

            return result;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
