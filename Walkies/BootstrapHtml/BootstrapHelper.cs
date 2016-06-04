using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Walkies.BootstrapHtml
{
    public static class BootstrapHelper
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        public static string LabelClass = "control-label";
        public static string ControlClass = "form-control";
        public static string DropDownListClass = "dropdown";
        public static string DropDownMenuClass = "dropdown-menu";
        public static string DropDownToggleClass = "dropdown-toggle";
        public static string ButtonClass = "btn";
        public static string CaretClass = "caret";
        public static string ButtonDefaultClass = "btn-default";
        public static string FormGoup = "form-group";
        public static string ButtonGoup = "btn-group";
        public static string DatePicker = "date-picker";
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Wraps a field with bootstrap markup
        /// </summary>
        /// <param name="html"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        public static MvcHtmlString WrapFieldGroup( MvcHtmlString fields, string @wrapperClass = "" )
        {

            var wrapper = new TagBuilder( "div" );
            wrapper.AddCssClass( BootstrapHelper.FormGoup );

            if( string.IsNullOrWhiteSpace( @wrapperClass ) == false )
            {
                wrapper.AddCssClass( @wrapperClass );
            }

            wrapper.InnerHtml = fields.ToString();

            return new MvcHtmlString( wrapper.ToString() );

        }

        /// <summary>
        /// Wraps a field with bootstrap markup
        /// </summary>
        /// <param name="html"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxBuilder<TModel>(  Bootstrap<TModel> bootstrap, Expression<Func<TModel, bool>> expression, object htmlAttributes = null )
        {

            var checkbox = new TagBuilder( "input" );
            var name = ExpressionHelper.GetExpressionText( expression );
            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            var compiled = expression.Compile();
            var value = compiled.Invoke( bootstrap.ViewData.Model );

            foreach( var attribute in attributes )
            {
                checkbox.MergeAttribute( attribute.Key, attribute.Value.ToString() );
            }

            checkbox.MergeAttribute( "name", name );
            checkbox.MergeAttribute( "id", name );
            checkbox.MergeAttribute( "type", "checkbox" );

            if( value )
            {
                checkbox.MergeAttribute( "checked", "" );
            }

            return new MvcHtmlString( checkbox.ToString() );

        }

        /// <summary>
        /// Wraps a field with bootstrap markup
        /// </summary>
        /// <param name="html"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        public static string WrapColumn( string html, string columnClass )
        {

            var wrapper = new TagBuilder( "div" );
            wrapper.AddCssClass( columnClass );
            wrapper.InnerHtml = html;

            return wrapper.ToString();

        }

        /// <summary>
        /// Wraps a field with bootstrap markup
        /// </summary>
        /// <param name="html"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        public static string WrapCheckbox( MvcHtmlString label, MvcHtmlString field, MvcHtmlString validation = null )
        {

            var wrapper = new TagBuilder( "div" );
            wrapper.AddCssClass( "checkbox" );
            
            wrapper.InnerHtml += field.ToString();
            wrapper.InnerHtml += label.ToString();

            return wrapper.ToString();

        }

        /// <summary>
        /// Wraps a field with bootstrap markup
        /// </summary>
        /// <param name="html"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        public static string WrapForInputButtonGroup<TModel>( this Bootstrap<TModel> bootstrap, string html )
        {

            if( bootstrap.IsGrouping == false ) return html;

            var wrapper = new TagBuilder( "div" );
            wrapper.AddCssClass( "input-group-btn" );
            wrapper.InnerHtml = html;

            return wrapper.ToString();

        }

        /// <summary>
        /// Wraps a field with bootstrap markup
        /// </summary>
        /// <param name="html"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        public static string IndentColumn( string indentClass )
        {

            return string.Format( "<div class=\"{0}\">&nbsp;</div>", indentClass );

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
