using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.Mappers;
using AutoMapper.QueryableExtensions;

using Walkies.AutoMapper;

namespace Walkies
{

    /// <summary>
    /// Provides additional helpers for mapping class instances.
    /// </summary>
    public static class AutoMap
    {

        private static object _lockObject = new object();

        /// <summary>
        /// Used to retain which mappings have already been created so that we don't create mappings more than once.
        /// </summary>
        private class CurrentMappings
        {


            /// <summary>
            /// The source type for the mapping.
            /// </summary>
            internal Type SourceType;
            /// <summary>
            /// The destination type for the mapping.
            /// </summary>
            internal Type DestinationType;

            /// <summary>
            /// Used to retain which mappings have already been created so that we don't create mappings more than once.
            /// </summary>
            /// <param name="sourceType">The source type for the mapping</param>
            /// <param name="destinationType">The destination type for the mapping</param>
            internal CurrentMappings( Type sourceType, Type destinationType )
            {
                SourceType = sourceType;
                DestinationType = destinationType;
            }

        }

        /// <summary>
        /// Maps data from one entity to another.
        /// </summary>
        /// <typeparam name="S">The type of the entity to map from</typeparam>
        /// <typeparam name="T">The type of the entity to map to</typeparam>
        /// <param name="source">The entity to map from</param>
        /// <param name="destination">The entity to map to</param>
        /// <param name="modelState">The model state for the form. Does not check if the state is valid</param>
        /// <param name="ignoreProperties">A set of property names to ignore</param>
        public static void Map<S, T>( S source, T destination, ModelStateDictionary modelState, List<string> ignoreProperties = null )
        {
            Type sType = ObjectContext.GetObjectType( source.GetType() );

            List<string> sourcePropertyNames = sType.GetProperties().Select( p => p.Name ).ToList();
            List<string> modelProperties = modelState.Keys.ToList();

            ignoreProperties = ignoreProperties ?? new List<string>();

            ignoreProperties.AddRange( sourcePropertyNames.Except( modelProperties ) );

            Map( source, destination, ignoreProperties );
        }

        /// <summary>
        /// Maps data from one entity to another.
        /// </summary>
        /// <typeparam name="S">The type of the entity to map from</typeparam>
        /// <typeparam name="T">The type of the entity to map to</typeparam>
        /// <param name="source">The entity to map from</param>
        /// <param name="destination">The entity to map to</param>
        /// <param name="ignoreProperties">A set of property names to ignore</param>
        public static void Map<S, T>( S source, T destination, IEnumerable<string> ignoreProperties = null )
        {

            ignoreProperties = ignoreProperties ?? new List<string>();

            Type sType = ObjectContext.GetObjectType( source.GetType() );
            Type tType = ObjectContext.GetObjectType( destination.GetType() );

            Type baseType = sType.BaseType;

            // Allows a base type to be passed through and used with the AutoMap.
            while ( baseType != typeof( object ) )
            {
                Mapper.CreateMap( baseType, tType );
                baseType = baseType.BaseType;
            }

            Mapper.CreateMap( sType, tType ).ResetIgnoreProperties( sType, tType )
                                            .IgnoreProperties( tType, ignoreProperties )
                                            .MapToMembers( sType, tType )
                                            .MapFromMembers( sType, tType );

            List<CurrentMappings> currentMappings = new List<CurrentMappings>();
            currentMappings.Add( new CurrentMappings( sType, tType ) );

            CreateSecondaryMap( currentMappings, sType );
            CreateSecondaryMap( currentMappings, tType );

            CreateChildToMappings( currentMappings, sType, tType, ignoreProperties );
            CreateChildFromMappings( currentMappings, sType, tType, ignoreProperties );

            Mapper.Map( source, destination );

        }

        private static void CreateSecondaryMap( List<CurrentMappings> currentMappings, Type mappingType )
        {
            var createMapAttributes = mappingType.GetCustomAttributes( typeof( CreateMapAttribute ) );

            foreach ( var createMap in createMapAttributes )
            {
                CreateMapAttribute attribute = createMap as CreateMapAttribute;

                if ( MatchFound( currentMappings, attribute.SourceType, attribute.DestinationType ) == false )
                {
                    Mapper.CreateMap( attribute.SourceType, attribute.DestinationType );
                    currentMappings.Add( new CurrentMappings( attribute.SourceType, attribute.DestinationType ) );
                }
            }
        }

        /// <summary>
        /// Ignores mapping to properties on the destination class that have the attribute AutoMapIgnore.
        /// </summary>
        /// <param name="expression">The AutoMapper</param>
        /// <param name="mappingType">The destination type</param>
        /// <param name="ignoreProperties">A list of one or more property names to be ignored</param>
        /// <returns>The AutoMapper (for chaining)</returns>
        public static IMappingExpression IgnoreProperties( this IMappingExpression expression, Type mappingType, IEnumerable<string> ignoreProperties )
        {

            var propertiesWithAutoMapIgnoreAttribute = mappingType.GetProperties().Where( p => ignoreProperties.Contains( p.Name ) || p.GetCustomAttributes( typeof( AutoMapIgnoreAttribute ), true ).OfType<AutoMapIgnoreAttribute>().Count() > 0 );

            foreach ( var property in propertiesWithAutoMapIgnoreAttribute )
            {
                expression.ForMember( property.Name, opt => opt.Ignore() );
            }

            return expression;
        }

        /// <summary>
        /// Resets any previously added ignore properties for this mapping, by setting any mappable fields back
        /// to MapFrom.
        /// </summary>
        /// <param name="expression">The AutoMapper</param>
        /// <param name="mappingType">The destination type</param>
        /// <param name="ignoreProperties">A list of one or more property names to be ignored</param>
        /// <returns>The AutoMapper (for chaining)</returns>
        public static IMappingExpression ResetIgnoreProperties( this IMappingExpression expression, Type sType, Type tType )
        {

            var sProperties = sType.GetProperties( BindingFlags.Instance | BindingFlags.Public );
            foreach( var property in tType.GetProperties( BindingFlags.Instance | BindingFlags.Public ) )
            {
                if( sProperties.FirstOrDefault( p => p.Name == property.Name ) != null )
                {
                    expression.ForMember( property.Name, opt => opt.MapFrom( property.Name ) );
                }
            }

            return expression;
        }

        /// <summary>
        /// Creates a mapping definition for mapping properties of different names.
        /// </summary>
        /// <param name="expression">The AutoMapper</param>
        /// <param name="source">The source type</param>
        /// <param name="destination">The destination type</param>
        /// <returns>The AutoMapper (for chaining)</returns>
        public static IMappingExpression MapToMembers( this IMappingExpression expression, Type source, Type destination )
        {
            var propertiesWithMapsToAttribute = source.GetProperties().Where( p => p.GetCustomAttributes( typeof( MapsToAttribute ), true ).OfType<MapsToAttribute>().Count() > 0 );

            foreach ( var property in propertiesWithMapsToAttribute )
            {
                expression.ForMember( property.GetCustomAttributes( typeof( MapsToAttribute ), true ).OfType<MapsToAttribute>().First().MapsTo, opt => opt.MapFrom( property.Name ) );
            }

            return expression;
        }

        /// <summary>
        /// Creates a mapping definition for mapping properties of different names.
        /// </summary>
        /// <param name="expression">The AutoMapper</param>
        /// <param name="source">The source type</param>
        /// <param name="destination">The destination type</param>
        /// <returns>The AutoMapper (for chaining)</returns>
        public static IMappingExpression MapFromMembers( this IMappingExpression expression, Type source, Type destination )
        {
            var propertiesWithMapsToAttribute = destination.GetProperties().Where( p => p.GetCustomAttributes( typeof( MapsFromAttribute ), true ).OfType<MapsFromAttribute>().Count() > 0 );

            foreach ( var property in propertiesWithMapsToAttribute )
            {
                expression.ForMember( property.Name, opt => opt.MapFrom( property.GetCustomAttributes( typeof( MapsFromAttribute ), true ).OfType<MapsFromAttribute>().First().MapsFrom ) );
            }

            return expression;
        }

        /// <summary>
        /// Creates a mapping definition.
        /// </summary>
        /// <param name="currentMappings">A list containing the map definitions that have already been created</param>
        /// <param name="source">The source type</param>
        /// <param name="destination">The destination type</param>
        private static void CreateChildToMappings( List<CurrentMappings> currentMappings, Type source, Type destination, IEnumerable<string> ignoreProperties )
        {

            var propertiesWithCreateChildMapAttribute = source.GetProperties().Where( p => p.GetCustomAttributes( typeof( CreateChildMapToAttribute ), true ).OfType<CreateChildMapToAttribute>().Count() > 0 );

            foreach ( var property in propertiesWithCreateChildMapAttribute )
            {

                Type sType = property.PropertyType;
                Type tType = property.GetCustomAttributes( typeof( CreateChildMapToAttribute ), true ).OfType<CreateChildMapToAttribute>().First().DestinationType;

                Mapper.CreateMap( sType, tType ).IgnoreProperties( tType, ignoreProperties ).MapToMembers( sType, tType ).MapFromMembers( sType, tType );

                if ( MatchFound( currentMappings, sType, tType ) == false )
                {
                    currentMappings.Add( new CurrentMappings( sType, tType ) );
                    CreateChildToMappings( currentMappings, sType, tType, ignoreProperties );
                }

            }

        }

        /// <summary>
        /// Creates a mapping definition.
        /// </summary>
        /// <param name="currentMappings">A list containing the map definitions that have already been created</param>
        /// <param name="source">The source type</param>
        /// <param name="destination">The destination type</param>
        private static void CreateChildFromMappings( List<CurrentMappings> currentMappings, Type source, Type destination, IEnumerable<string> ignoreProperties )
        {

            var propertiesWithCreateChildMapAttribute = destination.GetProperties().Where( p => p.GetCustomAttributes( typeof( CreateChildMapFromAttribute ), true ).OfType<CreateChildMapFromAttribute>().Count() > 0 );

            foreach ( var property in propertiesWithCreateChildMapAttribute )
            {

                Type tType = property.PropertyType;
                Type sType = property.GetCustomAttributes( typeof( CreateChildMapFromAttribute ), true ).OfType<CreateChildMapFromAttribute>().First().SourceType;

                Mapper.CreateMap( sType, tType ).IgnoreProperties( tType, ignoreProperties ).MapToMembers( sType, tType ).MapFromMembers( sType, tType );
                
                if ( MatchFound( currentMappings, sType, tType ) == false )
                {
                    currentMappings.Add( new CurrentMappings( sType, tType ) );
                    CreateChildFromMappings( currentMappings, sType, tType, ignoreProperties );
                }

            }

        }

        /// <summary>
        /// Determines if a mapping has already occurred between the source type and the destination type.
        /// </summary>
        /// <param name="currentMappings">A list containing the map definitions that have already been created</param>
        /// <param name="sType">The source type</param>
        /// <param name="tType">The destination type</param>
        /// <returns></returns>
        private static bool MatchFound( List<CurrentMappings> currentMappings, Type sType, Type tType )
        {
            return ( currentMappings.FirstOrDefault( cm => cm.SourceType == sType && cm.DestinationType == tType ) != null );
        }
        
        public static void AttachEnumDescriptionMapper()
        {
            MapperRegistry.Mappers.Insert( 0, new EnumDescriptionMapper() );
        }

    }

}