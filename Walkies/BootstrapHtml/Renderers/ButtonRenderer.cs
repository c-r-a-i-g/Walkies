using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Walkies.BootstrapHtml.Renderers
{
    public static class ButtonRenderer
    {

        public static MvcHtmlString Button( this HtmlHelper html, string label, ButtonStyle buttonStyle, RouteValueDictionary htmlAttributes, string onClick )
        {
            return ButtonHelper( html, label, buttonStyle, htmlAttributes, onClick, "", "button" );
        }

        public static MvcHtmlString LinkButton( this HtmlHelper html, string label, ButtonStyle buttonStyle, RouteValueDictionary htmlAttributes, string onClick )
        {
            var javascript = "";
            var href = "";
            NormaliseOnClick( onClick, out javascript, out href );
            return ButtonHelper( html, label, buttonStyle, htmlAttributes, javascript, href, "a" );
        }

        public static MvcHtmlString LinkButton( this HtmlHelper html, string label, ButtonStyle buttonStyle, RouteValueDictionary htmlAttributes, string action, string controller, object routeValues = null )
        {
            if( routeValues == null )
            {
                routeValues = new { };
            }

            var routeValueDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes( routeValues );
            var href = GenerateUrl( html.ViewContext.RequestContext, html.RouteCollection, null, action, controller, null, null, null, routeValueDictionary, false );
            return ButtonHelper( html, label, buttonStyle, htmlAttributes, "", href, "a" );
        }

        public static MvcHtmlString LinkButton( this HtmlHelper html, string label, ButtonStyle buttonStyle, RouteValueDictionary htmlAttributes, string action, string controller, string area, object routeValues = null )
        {

            if( routeValues == null )
            {
                routeValues = new { };
            }

            var routeValueDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes( routeValues );

            if( routeValueDictionary.ContainsKey( "Area" ) )
            {
                routeValueDictionary[ "Area" ] = area;
            }

            else
            {
                routeValueDictionary.Add( "Area", area );
            }

            var href = GenerateUrl( html.ViewContext.RequestContext, html.RouteCollection, null, action, controller, null, null, null, routeValueDictionary, false );
            return ButtonHelper( html, label, buttonStyle, htmlAttributes, "", href, "a" );

        }

        public static MvcHtmlString Link( this HtmlHelper html, string label, RouteValueDictionary htmlAttributes, string href )
        {
            return LinkButton( html, label, ButtonStyle.None, htmlAttributes, href );
        }

        public static MvcHtmlString Link( this HtmlHelper html, string label, RouteValueDictionary htmlAttributes, string action, string controller, object routeValues = null )
        {
            return LinkButton( html, label, ButtonStyle.None, htmlAttributes, action, controller, routeValues );
        }

        public static MvcHtmlString Link( this HtmlHelper html, string label, RouteValueDictionary htmlAttributes, string action, string controller, string area, object routeValues = null )
        {
            return LinkButton( html, label, ButtonStyle.None, htmlAttributes, action, controller, area, routeValues );
        }

        private static void NormaliseOnClick( string onClick, out string javascript, out string href )
        {

            var normalised = onClick.Trim().ToLower();
            
            if( normalised.StartsWith( "http" ) || normalised.StartsWith( "/" ) )
            {
                javascript = "";
                href = onClick;
            }

            else if( normalised.StartsWith( "javascript:" ) || normalised.EndsWith( ")" ) || normalised.EndsWith( ";" ) )
            {
                javascript = onClick;
                href = "";
            }

            else
            {
                javascript = onClick;
                href = "";
            }

        }

        private static MvcHtmlString ButtonHelper( HtmlHelper html, string label, ButtonStyle buttonStyle, RouteValueDictionary htmlAttributes, string javascript, string href, string element )
        {

            TagBuilder tagBuilder = new TagBuilder( element );
            tagBuilder.AddCssClass( buttonStyle.GetDescription() );
            tagBuilder.SetInnerText( label );

            if( element == "a" )
            {

                if( htmlAttributes.ContainsKey( "role" ) == false && string.IsNullOrWhiteSpace( javascript ) == false )
                {
                    htmlAttributes.Add( "role", "button" );
                }

                if( htmlAttributes.ContainsKey( "href" ) == false && string.IsNullOrWhiteSpace( href ) )
                {
                    htmlAttributes.Add( "href", "javascript:void(0)" );
                }

                else if( string.IsNullOrWhiteSpace( href ) == false )
                {
                    htmlAttributes.Add( "href", href );
                }
            
            }

            if( string.IsNullOrWhiteSpace( javascript ) == false )
            {
                htmlAttributes.Add( "onclick", javascript );
            }

            foreach( var item in htmlAttributes )
            {
                if( tagBuilder.Attributes.ContainsKey( item.Key ) )
                {
                    tagBuilder.Attributes[ item.Key ] += " " + item.Value.ToString();
                }

                else
                {
                    tagBuilder.Attributes.Add( item.Key, item.Value.ToString() );
                }
            }

            return new MvcHtmlString( tagBuilder.ToString() );

        }

        private static string GenerateUrl( RequestContext requestContext, RouteCollection routeCollection, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, bool includeImplicitMvcValues )
        {
            string url = UrlHelper.GenerateUrl( routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues );
            return url;
        }

        private static string GenerateUrl( RequestContext requestContext, RouteCollection routeCollection, string routeName, RouteValueDictionary routeValues )
        {
            return GenerateUrl( requestContext, routeCollection, routeName, null /* protocol */, null /* hostName */, null /* fragment */, routeValues );
        }

        private static string GenerateUrl( RequestContext requestContext, RouteCollection routeCollection, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues )
        {
            return GenerateUrl( requestContext, routeCollection, routeName, null /* actionName */, null /* controllerName */, protocol, hostName, fragment, routeValues, false /* includeImplicitMvcValues */);
        }
    
    }
}