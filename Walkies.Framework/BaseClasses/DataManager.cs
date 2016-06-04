using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Walkies;

namespace Walkies.Framework.BaseClasses
{
    public class DataManager
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
        /// Tries to set the active deliveredState of an newEntity by attempting to create an instance of its manager and
        /// calling the SetActiveState function on it.
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <param name="primaryKey"></param>
        /// <param name="activeState"></param>
        public SaveState TrySetActiveState( string entityTypeName, string primaryKey, bool activeState )
        {

            dynamic manager = DataManager.GetManagerForEntity( entityTypeName );
            if( manager == null ) return SaveState.Error;

            try
            {

                var guidKey = Guid.Empty;
                var intKey = 0;
                object entity = null;

                if( Guid.TryParse( primaryKey, out guidKey ) )
                {
                    entity = manager.SetActiveState( guidKey, activeState );
                }

                else if( int.TryParse( primaryKey, out intKey ) )
                {
                    entity = manager.SetActiveState( intKey, activeState );
                }

                else
                {
                    entity = manager.SetActiveState( primaryKey, activeState );
                }

                var result = SaveState.Successful;
                string name = this.TryToGetEntityName( entity );
                if( string.IsNullOrEmpty( name ) == false )
                {
                    result.Data = name;
                }

                return result;

            }

            catch( Exception ) 
            {
                return SaveState.Error;
            }

        }

        /// <summary>
        /// Tries to make a copy of the entity identified by the given key, by attempting to create an 
        /// instance of its manager and calling the Copy function on it.
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <param name="primaryKey"></param>
        public SaveState TryCopy( string entityTypeName, string primaryKey )
        {

            dynamic manager = DataManager.GetManagerForEntity( entityTypeName );
            if( manager == null ) return SaveState.Error;

            try
            {

                var result = SaveState.Successful;
                var guidKey = Guid.Empty;
                var intKey = 0;

                if( Guid.TryParse( primaryKey, out guidKey ) )
                {
                    var originalEntity = manager.Find( guidKey );
                    result = manager.Copy( originalEntity );
                }

                else if( int.TryParse( primaryKey, out intKey ) )
                {
                    var originalEntity = manager.Find( intKey );
                    result = manager.Copy( originalEntity );
                }

                else
                {
                    var originalEntity = manager.Find( primaryKey );
                    result = manager.Copy( originalEntity );
                }

                string name = this.TryToGetEntityName( result.Entity );
                if( string.IsNullOrEmpty( name ) == false )
                {
                    result.Data = name;
                }

                return result;

            }

            catch( Exception )
            {
                return SaveState.Error;
            }

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// Attempts to create an instance of a data manager for the specified newEntity name.  The method assumes that
        /// the manager class is the newEntity name plus "Manager", i.e. the Group newEntity will have a manager called
        /// "GroupManager".  The newEntity name is case sensitive and must respect the casing of the types name.
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <returns></returns>
        public static dynamic GetManagerForEntity( string entityTypeName )
        {

            var managerName = entityTypeName + "Manager";
            var assembly = Assembly.GetExecutingAssembly();
            var managerType = assembly.GetTypes().FirstOrDefault( t => t.Name == managerName );

            if( managerType == null ) return null;
            
            try
            {
                return Activator.CreateInstance( managerType );
            }

            catch( Exception )
            {
                return null;
            }

        }

        /// <summary>
        /// Gets an English normalised version of a type name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string NormaliseTypeName( Type type )
        {
            if( type.Module.ScopeName == "EntityProxyModule" )
            {
                type = type.BaseType; // The type is an EF wrapper, so we need the underlying type
            }
            return NormaliseTypeName( type.Name );
        }

        /// <summary>
        /// Gets an English normalised version of a type name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string NormaliseTypeName( string typeName )
        {

            string result = "";
            char lastChar = ' ';

            foreach( char @char in typeName )
            {
                if( char.IsUpper( @char ) && char.IsLower( lastChar ) )
                {
                    result += " " + @char;
                }

                else if( char.IsLetterOrDigit( @char ) == false )
                {
                    result += " ";
                }

                else
                {
                    result += @char;
                }
                lastChar = @char;
            }

            return result;

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Tries to get the name property from an newEntity using the EntityNameAttribute
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        private string TryToGetEntityName( object entity )
        {
            try
            {
                return entity.GetValueOfPropertyWithAttribute<EntityNameAttribute>( "" ).ToString();
            }

            catch( Exception )
            {
                return "";
            }
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
