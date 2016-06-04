using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Walkies.BootstrapHtml
{
    public static class FormRenderer
    {

        public static MvcForm BeginForm<TModel>( this Bootstrap<TModel> bootstrap, FormLayout formLayout = null, object htmlAttributes = null )
        {

            formLayout = formLayout ?? FormLayout.Vertical();
            bootstrap.FormLayout = formLayout;
            bootstrap.Html.ViewContext.TempData[ "form-layout" ] = formLayout;

            var attributes = Bootstrap.GetAttributes( htmlAttributes, formLayout.FormStyle.GetDescription() );
            var formAction = bootstrap.Html.ViewContext.HttpContext.Request.RawUrl;

            return FormHelper<TModel>( bootstrap, formAction, FormMethod.Post, attributes );

        }

        public static MvcForm BeginProtectedForm<TModel>( this Bootstrap<TModel> bootstrap, FormLayout formLayout = null, object htmlAttributes = null )
        {

            formLayout = formLayout ?? FormLayout.Vertical();
            bootstrap.FormLayout = formLayout;
            bootstrap.Html.ViewContext.TempData[ "form-layout" ] = formLayout;

            var attributes = Bootstrap.GetAttributes( htmlAttributes, formLayout.FormStyle.GetDescription() );
            attributes.Add( "protect-data", "" );
            var formAction = bootstrap.Html.ViewContext.HttpContext.Request.RawUrl;

            return FormHelper<TModel>( bootstrap, formAction, FormMethod.Post, attributes );

        }

        private static MvcForm FormHelper<TModel>( this Bootstrap<TModel> bootstrap, string formAction, FormMethod method, IDictionary<string, object> htmlAttributes )
        {

            TagBuilder tagBuilder = new TagBuilder( "form" );
            tagBuilder.MergeAttributes( htmlAttributes );

            // action is implicitly generated, so htmlAttributes take precedence.
            tagBuilder.MergeAttribute( "action", formAction );

            // method is an explicit parameter, so it takes precedence over the htmlAttributes.
            tagBuilder.MergeAttribute( "method", HtmlHelper.GetFormMethodString( method ), true );

            bool traditionalJavascriptEnabled = bootstrap.Html.ViewContext.ClientValidationEnabled
                                                && !bootstrap.Html.ViewContext.UnobtrusiveJavaScriptEnabled;

            if( traditionalJavascriptEnabled )
            {
                // forms must have an ID for client validation
                tagBuilder.GenerateId( "form" );
            }

            bootstrap.Html.ViewContext.Writer.Write( tagBuilder.ToString( TagRenderMode.StartTag ) );
            var theForm = new MvcForm( bootstrap.Html.ViewContext );

            if( traditionalJavascriptEnabled )
            {
                bootstrap.Html.ViewContext.FormContext.FormId = tagBuilder.Attributes[ "id" ];
            }

            FormRenderer.WritePersistedProperties( bootstrap );

            return theForm;
        }

        private static void WritePersistedProperties<TModel>( Bootstrap<TModel> bootstrap )
        {

            var model = bootstrap.ViewData.Model;
            var type = typeof( TModel );
            var properties = type.GetProperties();

            foreach( var property in properties.Where( p => Attribute.IsDefined( p, typeof( PersistAttribute ), false ) ) )
            {
                var hiddenField = bootstrap.Html.Hidden( property.Name, property.GetValue( model ) );
                bootstrap.Html.ViewContext.Writer.Write( hiddenField.ToString() );
            }

            foreach( var property in properties.Where( p => p.GetType().IsValueType == false ) )
            {

                var subValue = property.GetValue( model );
                var subProperties = property.PropertyType.GetProperties();

                foreach( var subProperty in subProperties.Where( p => Attribute.IsDefined( p, typeof( PersistAttribute ), false ) ) )
                {
                    var hiddenField = bootstrap.Html.Hidden( property.Name + "_" + subProperty.Name, subProperty.GetValue( subValue ) );
                    bootstrap.Html.ViewContext.Writer.Write( hiddenField.ToString() );
                }

            }

        }

    }
}
