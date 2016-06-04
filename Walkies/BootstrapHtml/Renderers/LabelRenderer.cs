using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using Walkies.BootstrapHtml.FormExtensions;

namespace Walkies.BootstrapHtml.Renderers
{
    public static class LabelRenderer
    {

        public static MvcHtmlString Label( this HtmlHelper html, string expression, bool createEmptyLabel = true, string @class = "" )
        {
            return html.Label( expression, null, createEmptyLabel, @class );
        }

        public static MvcHtmlString Label( this HtmlHelper html, string expression, string labelText, bool createEmptyLabel = true, string @class = "" )
        {
            return LabelHelper( html, ModelMetadata.FromStringExpression( expression, html.ViewData ), expression, labelText, createEmptyLabel, @class );
        }

        public static MvcHtmlString LabelFor<TModel, TValue>( this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool createEmptyLabel = true, string @class = "" )
        {
            return html.LabelFor<TModel, TValue>( expression, null, createEmptyLabel, @class );
        }

        public static MvcHtmlString LabelFor<TModel, TValue>( this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, bool createEmptyLabel = true, string @class = "" )
        {
            return LabelHelper( html, ModelMetadata.FromLambdaExpression<TModel, TValue>( expression, html.ViewData ), ExpressionHelper.GetExpressionText( expression ), labelText, createEmptyLabel, @class );
        }

        public static MvcHtmlString LabelForModel( this HtmlHelper html, bool createEmptyLabel = true, string @class = "" )
        {
            return html.LabelForModel( null, createEmptyLabel, @class );
        }

        public static MvcHtmlString LabelForModel( this HtmlHelper html, string labelText, bool createEmptyLabel = true, string @class = "" )
        {
            return LabelHelper( html, html.ViewData.ModelMetadata, string.Empty, labelText, createEmptyLabel, @class );
        }

        private static MvcHtmlString LabelHelper( HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText = null, bool createEmptyLabel = true, string @class = "" )
        {

            var form = html.GetFormLayout();
            var displayName = metadata.DisplayName;

            if( string.IsNullOrEmpty( displayName ) )
            {
                var type = html.ViewData.Model.GetType();
                var propertyInfo = type.GetProperty( htmlFieldName );
                if( propertyInfo != null )
                {
                    var displayAttribute = propertyInfo.GetAttribute<DisplayAttribute>();
                    if( displayAttribute != null && string.IsNullOrEmpty( displayAttribute.Name ) == false )
                    {
                        displayName = displayAttribute.Name;
                    }
                }
            }

            string str = labelText ?? ( displayName ?? ( metadata.PropertyName ?? htmlFieldName.Split( new char[] { '.' } ).Last<string>() ) );
            
            if( string.IsNullOrWhiteSpace( str ) && createEmptyLabel == false )
            {
                return new MvcHtmlString( "" );
            }

            if( string.IsNullOrWhiteSpace( str ) )
            {
                if( form.FormStyle == FormStyle.Horizontal && string.IsNullOrWhiteSpace( form.LabelClass ) == false )
                {
                    return new MvcHtmlString( BootstrapHelper.IndentColumn( form.LabelClass ) );
                }
                else
                {
                    return new MvcHtmlString( "" );
                }
            }

            if( str.ToLower() == "&nbsp;" ) str = string.Empty;
            if( string.IsNullOrWhiteSpace( str ) ) return new MvcHtmlString( "" );

            TagBuilder tagBuilder = new TagBuilder( "label" );
            tagBuilder.Attributes.Add( "for", TagBuilder.CreateSanitizedId( html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName( htmlFieldName ) ) );
            tagBuilder.SetInnerText( str );


            if( string.IsNullOrWhiteSpace( @class ) == false )
            {
                if( tagBuilder.Attributes.ContainsKey( "class" ) == false )
                {
                    tagBuilder.Attributes.Add( "class", @class );
                }
                else
                {
                    tagBuilder.Attributes[ "class" ] = tagBuilder.Attributes[ "class" ] + " " + @class;
                }
            }
            
            return new MvcHtmlString( tagBuilder.ToString() );

        }

    }
}