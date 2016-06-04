using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Walkies.BootstrapHtml.Renderers
{
    public static class SelectListRenderer
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public static MvcHtmlString DropDownListWithDataFor<TModel, TProperty>( this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItemWithData> selectList, string optionLabel, RouteValueDictionary htmlAttributes )
        {
            return SelectInternal( htmlHelper, optionLabel, ExpressionHelper.GetExpressionText( expression ), selectList, false /* allowMultiple */, htmlAttributes );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal static string ListItemToOption( SelectListItemWithData item )
        {

            TagBuilder builder = new TagBuilder( "option" )
            {
                InnerHtml = HttpUtility.HtmlEncode( item.Text )
            };

            if( item.Value != null )
            {
                builder.Attributes[ "value" ] = item.Value;
            }

            if( item.Selected )
            {
                builder.Attributes[ "selected" ] = "selected";
            }

            foreach( var attribute in item.Attributes )
            {
                builder.Attributes[ "data-" + attribute.Key.ToCSSClass() ] = attribute.Value;
            }

            return builder.ToString( TagRenderMode.Normal );

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="optionLabel"></param>
        /// <param name="name"></param>
        /// <param name="selectList"></param>
        /// <param name="allowMultiple"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private static MvcHtmlString SelectInternal( this HtmlHelper html, string optionLabel, string name, IEnumerable<SelectListItemWithData> selectList, bool allowMultiple, RouteValueDictionary htmlAttributes )
        {

            string fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName( name );

            if( String.IsNullOrEmpty( fullName ) )
                throw new ArgumentException( "No name" );

            if( selectList == null )
                throw new ArgumentException( "No selectlist" );

            object defaultValue = ( allowMultiple ) ? GetModelStateValue( html, fullName, typeof( string[] ) ) : GetModelStateValue( html, fullName, typeof( string ) );

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if( defaultValue == null )
                defaultValue = html.ViewData.Eval( fullName );

            if( defaultValue != null )
            {
                IEnumerable defaultValues = ( allowMultiple ) ? defaultValue as IEnumerable : new[] { defaultValue };
                IEnumerable<string> values = from object value in defaultValues select Convert.ToString( value, CultureInfo.CurrentCulture );
                HashSet<string> selectedValues = new HashSet<string>( values, StringComparer.OrdinalIgnoreCase );
                var newSelectList = new List<SelectListItemWithData>();

                foreach( var item in selectList )
                {
                    item.Selected = ( item.Value != null ) ? selectedValues.Contains( item.Value ) : selectedValues.Contains( item.Text );
                    newSelectList.Add( item );
                }
                selectList = newSelectList;
            }

            // Convert each ListItem to an <option> tag
            StringBuilder listItemBuilder = new StringBuilder();

            // Make optionLabel the first item that gets rendered.
            if( optionLabel != null )
                listItemBuilder.Append( ListItemToOption( new SelectListItemWithData() { Text = optionLabel, Value = String.Empty, Selected = false } ) );

            foreach( var item in selectList )
            {
                listItemBuilder.Append( ListItemToOption( item ) );
            }

            TagBuilder tagBuilder = new TagBuilder( "select" )
            {
                InnerHtml = listItemBuilder.ToString()
            };

            tagBuilder.MergeAttributes( htmlAttributes );
            tagBuilder.MergeAttribute( "name", fullName, true /* replaceExisting */);
            tagBuilder.GenerateId( fullName );
            if( allowMultiple )
                tagBuilder.MergeAttribute( "multiple", "multiple" );

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if( html.ViewData.ModelState.TryGetValue( fullName, out modelState ) )
            {
                if( modelState.Errors.Count > 0 )
                {
                    tagBuilder.AddCssClass( HtmlHelper.ValidationInputCssClassName );
                }
            }

            tagBuilder.MergeAttributes( html.GetUnobtrusiveValidationAttributes( name ) );

            return MvcHtmlString.Create( tagBuilder.ToString( TagRenderMode.Normal ) );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        private static object GetModelStateValue( HtmlHelper htmlHelper, string key, Type destinationType )
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
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



