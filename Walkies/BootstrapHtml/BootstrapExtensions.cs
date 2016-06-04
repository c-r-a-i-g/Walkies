using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Walkies.BootstrapHtml.Renderers;

namespace Walkies.BootstrapHtml.FormExtensions
{
    public static class BootstrapExtensions
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
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> Label<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null )
        {
            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            bootstrap.Append( label.ToString() );
            return bootstrap;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> TextBox<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {
            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.TextBoxFor( expression, Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass ) );
            bootstrap.Append( field.ToString() );
            return bootstrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> TextBoxField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.TextBoxFor( expression, Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass ) );
            var valid = bootstrap.Html.ValidationMessageFor( expression ) ?? new MvcHtmlString( "" );
            
            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + BootstrapHelper.WrapColumn( valid.ToString() + field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> Money<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {
            RouteValueDictionary attributes = Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass );

            if ( attributes.ContainsKey( "mask-money" ) == false )
            {
                attributes.Add( "mask-money", "" );
            }

            if ( attributes.ContainsKey( "data-allow-zero" ) == false )
            {
                attributes.Add( "data-allow-zero", "true" );
            }

            if ( attributes.ContainsKey( "autocomplete" ) == false )
            {
                attributes.Add( "autocomplete", "off" );
            }

            string value = "";

            if ( bootstrap.Html.ViewData.Model != null )
            {
                value = expression.Compile()( bootstrap.Html.ViewData.Model ).ToString().ToLower();

                decimal decimalValue;
                bool isNumber = decimal.TryParse( value, out decimalValue );

                if ( isNumber )
                {
                    value = decimalValue.ToString( "N2" );
                }
            }

            string inputBox = bootstrap.Html.TextBoxFor( expression, attributes ).ToString();
            string field = Regex.Replace( inputBox, "value=\"(.*?)\"", "value=\"" + value + "\"" );

            var form = bootstrap.FormLayout;
            var span = "<span class=\"currency-symbol\">$</span>";
            bootstrap.Append( field.ToString() + span );
            return bootstrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> MoneyField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {

            RouteValueDictionary attributes = Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass );

            if ( attributes.ContainsKey( "mask-money" ) == false )
            {
                attributes.Add( "mask-money", "" );
            }

            if ( attributes.ContainsKey( "data-allow-zero" ) == false )
            {
                attributes.Add( "data-allow-zero", "true" );
            }

            if ( attributes.ContainsKey( "autocomplete" ) == false )
            {
                attributes.Add( "autocomplete", "off" );
            }

            string value = "";

            if ( bootstrap.Html.ViewData.Model != null )
            {
                value = expression.Compile()( bootstrap.Html.ViewData.Model ).ToString().ToLower();

                decimal decimalValue;
                bool isNumber = decimal.TryParse( value, out decimalValue );

                if ( isNumber )
                {
                    value = decimalValue.ToString( "N2" );
                }
            }

            string inputBox = bootstrap.Html.TextBoxFor( expression, attributes ).ToString();
            string field = Regex.Replace( inputBox, "value=\"(.*?)\"", "value=\"" + value + "\"" );

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var valid = bootstrap.Html.ValidationMessageFor( expression ) ?? new MvcHtmlString( "" );

            var span = "<span class=\"currency-symbol\">$</span>";

            var elements = "";

            bootstrap.FormGroup();

            if ( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString() + span;
            }

            else
            {
                elements = label.ToString() + BootstrapHelper.WrapColumn( valid.ToString() + field.ToString() + span, form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> TextArea<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {
            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.TextAreaFor( expression, Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass ) );
            bootstrap.Append( field.ToString() );
            return bootstrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> TextAreaField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.TextAreaFor( expression, Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass ) );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + BootstrapHelper.WrapColumn( valid.ToString() + field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> Password<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {
            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.PasswordFor( expression, Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass ) );
            bootstrap.Append( field.ToString() );
            return bootstrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> PasswordField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.PasswordFor( expression, Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass ) );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + BootstrapHelper.WrapColumn( valid.ToString() + field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DatePicker<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, DatePickerOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {


            var attributes = Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass );
            attributes.Add( "date-picker", "" );

            options = options ?? new DatePickerOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            string value = "";

            if ( bootstrap.Html.ViewData.Model != null )
            {
                value = expression.Compile()( bootstrap.Html.ViewData.Model ).ToString().ToLower();

                DateTime dateValue = DateTime.MinValue;
                bool isDate = DateTime.TryParse( value, out dateValue );

                if ( isDate )
                {
                    value = dateValue.ToString( "dd/MM/yyyy" );
                }
            }

            string inputBox = bootstrap.Html.TextBoxFor( expression, "{0:" + options.Format + "}", attributes ).ToString();
            string inputBoxWithUpdatedValue = Regex.Replace( inputBox, "value=\"(.*?)\"", "value=\"" + value + "\"" );

            var form = bootstrap.FormLayout;
            var field = "<div class=\"calendar-wrapper\">" + inputBoxWithUpdatedValue + "<i class=\"glyphicon glyphicon-calendar fa fa-calendar\"></i></div>";
            bootstrap.Append( field.ToString() );
            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DatePickerField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, DatePickerOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass );
            attributes.Add( "date-picker", "" );

            options = options ?? new DatePickerOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            string value = "";

            if ( bootstrap.Html.ViewData.Model != null )
            {
                value = expression.Compile()( bootstrap.Html.ViewData.Model ).ToString().ToLower();

                DateTime dateValue = DateTime.MinValue;
                bool isDate = DateTime.TryParse( value, out dateValue );

                if ( isDate )
                {
                    value = dateValue.ToString( "dd/MM/yyyy" );
                }
            }

            string inputBox = bootstrap.Html.TextBoxFor( expression, "{0:" + options.Format + "}", attributes ).ToString();
            string inputBoxWithUpdatedValue = Regex.Replace( inputBox, "value=\"(.*?)\"", "value=\"" + value + "\"" );

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = "<div class=\"calendar-wrapper\">" + inputBoxWithUpdatedValue + "<i class=\"glyphicon glyphicon-calendar fa fa-calendar\"></i></div>";
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + BootstrapHelper.WrapColumn( valid.ToString() + field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DateRangePicker<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, DateRangePickerOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass );
            attributes.Add( "date-range-picker", "" );

            options = options ?? new DateRangePickerOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            //TODO: Remove
            attributes.Add( "_vkenabled", "false" );

            string value = "";

            if ( bootstrap.Html.ViewData.Model != null )
            {
                value = expression.Compile()( bootstrap.Html.ViewData.Model ).ToString().ToLower();

                DateTime dateValue = DateTime.MinValue;
                bool isDate = DateTime.TryParse( value, out dateValue );

                if ( isDate )
                {
                    value = dateValue.ToString( "dd/MM/yyyy" );
                }
            }

            string inputBox = bootstrap.Html.TextBoxFor( expression, attributes ).ToString();
            string inputBoxWithUpdatedValue = Regex.Replace( inputBox, "value=\"(.*?)\"", "value=\"" + value + "\"" );

            var form = bootstrap.FormLayout;
            var field = "<div class=\"calendar-wrapper\">" + inputBoxWithUpdatedValue + "<i class=\"glyphicon glyphicon-calendar fa fa-calendar\"></i></div>";
            bootstrap.Append( field.ToString() );
            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DateRangePickerField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, DateRangePickerOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, BootstrapHelper.ControlClass );
            attributes.Add( "date-range-picker", "" );

            options = options ?? new DateRangePickerOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            //TODO: Remove
            attributes.Add( "_vkenabled", "false" );

            string value = "";
            
            if ( bootstrap.Html.ViewData.Model != null )
            {
                value = expression.Compile()( bootstrap.Html.ViewData.Model ).ToString().ToLower();

                DateTime dateValue = DateTime.MinValue;
                bool isDate = DateTime.TryParse( value, out dateValue );

                if ( isDate )
                {
                    value = dateValue.ToString( "dd/MM/yyyy" );
                }
            }
            
            string inputBox = bootstrap.Html.TextBoxFor( expression, attributes ).ToString();
            string inputBoxWithUpdatedValue = Regex.Replace( inputBox, "value=\"(.*?)\"", "value=\"" + value + "\"" );
            
            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = "<div class=\"calendar-wrapper\">" + inputBoxWithUpdatedValue + "<i class=\"glyphicon glyphicon-calendar fa fa-calendar\"></i></div>";
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + BootstrapHelper.WrapColumn( valid.ToString() + field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> CheckBox<TModel>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, bool>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression );
            var field = bootstrap.Html.CheckBoxFor( expression, htmlAttributes );
            var fieldColumn = BootstrapHelper.WrapCheckbox( label, field );

            bootstrap.Append( fieldColumn );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> CheckBoxWithLabel<TModel>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, bool>> expression, string label, object htmlAttributes = null, string wrapperClass = "" )
        {

            var form = bootstrap.FormLayout;
            var labelHtml = bootstrap.Html.LabelFor( expression, label );
            var field = bootstrap.Html.CheckBoxFor( expression, htmlAttributes );
            var fieldColumn = BootstrapHelper.WrapCheckbox( labelHtml, field );

            bootstrap.Append( fieldColumn );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> CheckBoxField<TModel>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, bool>> expression, object htmlAttributes = null, string wrapperClass = "" )
        {

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression );
            var field = bootstrap.Html.CheckBoxFor( expression, htmlAttributes );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var labelColumn = "";
            var fieldColumn = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                fieldColumn = BootstrapHelper.WrapCheckbox( label, field, valid );
            }

            else
            {
                labelColumn = BootstrapHelper.IndentColumn( form.LabelClass );
                fieldColumn = BootstrapHelper.WrapColumn( BootstrapHelper.WrapCheckbox( label, field, valid ), form.FieldClass );
            }

            bootstrap.Append( labelColumn + fieldColumn );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DropDownList<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> items, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.DropDownListFor( expression, items, defaultLabel, attributes );
            var elements = "";

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = field.ToString();
            }

            else
            {
                elements = BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DropDownListField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> items, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.DropDownListFor( expression, items, defaultLabel, attributes );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + valid.ToString() + BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DropDownList<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItemWithData> items, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.DropDownListWithDataFor( expression, items, defaultLabel, attributes );
            var elements = "";

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = field.ToString();
            }

            else
            {
                elements = BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DropDownListField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItemWithData> items, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.DropDownListWithDataFor( expression, items, defaultLabel, attributes );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + valid.ToString() + BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DropDownList<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, Type enumValues, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            if( enumValues.IsEnum == false )
            {
                throw new ArgumentException( "enumValues must be an enum type" );
            }

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var items = Enum.GetValues( enumValues )
                            .OfType<Enum>().ToList()
                            .Select( i => new SelectListItem() { Text = i.GetDescription(), Value = ( Convert.ToInt32( i ) ).ToString(), Selected = false } )
                            .ToList();

            var value = (Enum)bootstrap.GetExpressionValue( expression );
            var description = value.GetDescription();
            var selectedItem = items.FirstOrDefault( i => i.Text == description );
            if( selectedItem != null ) selectedItem.Selected = true;

            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.DropDownListFor( expression, items, defaultLabel, attributes );
            var elements = "";

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = field.ToString();
            }

            else
            {
                elements = BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            return bootstrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> DropDownListField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, Type enumValues, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            if( enumValues.IsEnum == false )
            {
                throw new ArgumentException( "enumValues must be an enum type" );
            }

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var items = Enum.GetValues( enumValues )
                            .OfType<Enum>().ToList()
                            .Select( i => new SelectListItem() { Text = i.GetDescription(), Value = ( Convert.ToInt32( i ) ).ToString(), Selected = false } )
                            .ToList();

            var value = (Enum)bootstrap.GetExpressionValue( expression );
            var description = value.GetDescription();
            var selectedItem = items.FirstOrDefault( i => i.Text == description );
            if( selectedItem != null ) selectedItem.Selected = true;

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.DropDownListFor( expression, items, defaultLabel, attributes );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + valid.ToString() + BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> MultiDropDownList<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> items, DropDownOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );
            attributes.Add( "multiple", "" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.DropDownListFor( expression, items, attributes );
            var elements = "";

            var fieldString = field.ToString();
            var value = bootstrap.GetExpressionValue( expression );
            if( value != null )
            {
                IEnumerable<string> selectedValues = ( (IEnumerable)value ).Cast<object>().Select( s => s.ToString() );
                foreach( var item in items )
                {
                    if( selectedValues.Contains( item.Value.ToString() ) )
                    {
                        var pattern = string.Format( "value=\"{0}\"", item.Value.ToString() );
                        fieldString = fieldString.Replace( pattern, pattern + " selected" );
                    }
                }
            }
            field = new MvcHtmlString( fieldString );

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = field.ToString();
            }

            else
            {
                elements = BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> MultiDropDownListField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> items, DropDownOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );
            attributes.Add( "multiple", "" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.DropDownListFor( expression, items, attributes );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var fieldString = field.ToString();
            var value = bootstrap.GetExpressionValue( expression );
            if( value != null )
            {
                IEnumerable<string> selectedValues = ( (IEnumerable)value ).Cast<object>().Select( s => s.ToString() );
                foreach( var item in items )
                {
                    if( selectedValues.Contains( item.Value.ToString() ) )
                    {
                        var pattern = string.Format( "value=\"{0}\"", item.Value.ToString() );
                        fieldString = fieldString.Replace( pattern, pattern + " selected" );
                    }
                }
            }
            field = new MvcHtmlString( fieldString );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + valid.ToString() + BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> MultiDropDownList<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItemWithData> items, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );
            attributes.Add( "multiple", "" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.DropDownListWithDataFor( expression, items, defaultLabel, attributes );
            var elements = "";

            var fieldString = field.ToString();
            var value = bootstrap.GetExpressionValue( expression );
            if( value != null )
            {
                IEnumerable<string> selectedValues = ( (IEnumerable)value ).Cast<object>().Select( s => s.ToString() );
                foreach( var item in items )
                {
                    if( selectedValues.Contains( item.Value.ToString() ) )
                    {
                        var pattern = string.Format( "value=\"{0}\"", item.Value.ToString() );
                        fieldString = fieldString.Replace( pattern, pattern + " selected" );
                    }
                }
            }
            field = new MvcHtmlString( fieldString );

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = field.ToString();
            }

            else
            {
                elements = BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> MultiDropDownListField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItemWithData> items, DropDownOptions options = null, string defaultLabel = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );
            attributes.Add( "multiple", "" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.DropDownListWithDataFor( expression, items, defaultLabel, attributes );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var fieldString = field.ToString();
            var value = bootstrap.GetExpressionValue( expression );
            if( value != null )
            {
                IEnumerable<string> selectedValues = ( (IEnumerable)value ).Cast<object>().Select( s => s.ToString() );
                foreach( var item in items )
                {
                    if( selectedValues.Contains( item.Value.ToString() ) )
                    {
                        var pattern = string.Format( "value=\"{0}\"", item.Value.ToString() );
                        fieldString = fieldString.Replace( pattern, pattern + " selected" );
                    }
                }
            }
            field = new MvcHtmlString( fieldString );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + valid.ToString() + BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> MultiDropDownList<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, Type enumValues, DropDownOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            if( enumValues.IsEnum == false )
            {
                throw new ArgumentException( "enumValues must be an enum type" );
            }

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );
            attributes.Add( "multiple", "" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var items = Enum.GetValues( enumValues )
                            .OfType<Enum>().ToList()
                            .Select( i => new SelectListItem() { Text = i.GetDescription(), Value = ( Convert.ToInt32( i ) ).ToString(), Selected = false } )
                            .ToList();

            var form = bootstrap.FormLayout;
            var field = bootstrap.Html.DropDownListFor( expression, items, attributes );
            var elements = "";

            var fieldString = field.ToString();
            var value = bootstrap.GetExpressionValue( expression );
            if( value != null )
            {
                IEnumerable<int> selectedValues = ( (IEnumerable)value ).Cast<int>();
                foreach( var item in items )
                {
                    if( selectedValues.Contains( int.Parse( item.Value ) ) )
                    {
                        var pattern = string.Format( "value=\"{0}\"", item.Value );
                        fieldString = fieldString.Replace( pattern, pattern + " selected" );
                    }
                }
            }
            field = new MvcHtmlString( fieldString );

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = field.ToString();
            }

            else
            {
                elements = BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            return bootstrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> MultiDropDownListField<TModel, TProperty>( this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression, Type enumValues, DropDownOptions options = null, object htmlAttributes = null, string wrapperClass = "" )
        {

            if( enumValues.IsEnum == false )
            {
                throw new ArgumentException( "enumValues must be an enum type" );
            }

            var attributes = Bootstrap.GetAttributes( htmlAttributes, "selectpicker" );
            attributes.Add( "multiple", "" );

            options = options ?? new DropDownOptions();
            foreach( var option in options.Attributes )
            {
                attributes.Add( option.Key, option.Value );
            }

            var items = Enum.GetValues( enumValues )
                            .OfType<Enum>().ToList()
                            .Select( i => new SelectListItem() { Text = i.GetDescription(), Value = ( Convert.ToInt32( i ) ).ToString(), Selected = false } )
                            .ToList();

            var form = bootstrap.FormLayout;
            var label = bootstrap.Html.LabelFor( expression, true, BootstrapHelper.LabelClass + " " + form.LabelClass );
            var field = bootstrap.Html.DropDownListFor( expression, items, attributes );
            var valid = bootstrap.Html.ValidationMessageFor( expression );

            var fieldString = field.ToString();
            var value = bootstrap.GetExpressionValue( expression );
            if( value != null )
            {
                IEnumerable<int> selectedValues = ( (IEnumerable)value ).Cast<int>();
                foreach( var item in items )
                {
                    if( selectedValues.Contains( int.Parse( item.Value ) ) )
                    {
                        var pattern = string.Format( "value=\"{0}\"", item.Value );
                        fieldString = fieldString.Replace( pattern, pattern + " selected" );
                    }
                }
            }
            field = new MvcHtmlString( fieldString );

            var elements = "";

            bootstrap.FormGroup();

            if( form.FormStyle == FormStyle.Vertical )
            {
                elements = label.ToString() + valid.ToString() + field.ToString();
            }

            else
            {
                elements = label.ToString() + valid.ToString() + BootstrapHelper.WrapColumn( field.ToString(), form.FieldClass );
            }

            bootstrap.Append( elements );
            bootstrap.EndFormGroup();

            return bootstrap;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> SubmitButton<TModel>( this Bootstrap<TModel> bootstrap, string label = "Submit", ButtonStyle buttonStyle = ButtonStyle.Default, object htmlAttributes = null )
        {

            var form = bootstrap.FormLayout;
            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            if( attributes.ContainsKey( "type" ) == false ) attributes.Add( "type", "submit" );
            var button = bootstrap.Html.Button( label, buttonStyle, attributes, "" );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> SubmitButton<TModel>( this Bootstrap<TModel> bootstrap, ButtonStyle buttonStyle = ButtonStyle.Default, object htmlAttributes = null )
        {
            return SubmitButton( bootstrap, "Submit", buttonStyle, htmlAttributes );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> SaveButton<TModel>( this Bootstrap<TModel> bootstrap, ButtonStyle buttonStyle = ButtonStyle.Primary, object htmlAttributes = null, string onClick = "" )
        {

            if( string.IsNullOrEmpty( onClick ) == false)
            {
                return Button( bootstrap, "Save", ButtonStyle.Primary, htmlAttributes, onClick );
            }

            var form = bootstrap.FormLayout;
            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            if( attributes.ContainsKey( "type" ) == false ) attributes.Add( "type", "submit" );
            var button = bootstrap.Html.Button( "Save", buttonStyle, attributes, "" );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );
            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> NextButton<TModel>( this Bootstrap<TModel> bootstrap, ButtonStyle buttonStyle = ButtonStyle.Primary, object htmlAttributes = null, string onClick = "" )
        {
            return Button( bootstrap, "Next", ButtonStyle.Primary, htmlAttributes, onClick );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> CancelButton<TModel>( this Bootstrap<TModel> bootstrap, ButtonStyle buttonStyle = ButtonStyle.Primary, object htmlAttributes = null, string onClick = "" )
        {
            return Button( bootstrap, "Cancel", ButtonStyle.Default, htmlAttributes, onClick );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> Button<TModel>( this Bootstrap<TModel> bootstrap, string label = "Save", ButtonStyle buttonStyle = ButtonStyle.Default, object htmlAttributes = null, string onClick = "" )
        {

            var form = bootstrap.FormLayout;
            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            if( attributes.ContainsKey( "type" ) == false ) attributes.Add( "type", "button" );
            var button = bootstrap.Html.Button( label, buttonStyle, attributes, onClick );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> LinkButton<TModel>( this Bootstrap<TModel> bootstrap, string label, string onClick, ButtonStyle buttonStyle = ButtonStyle.Link, object htmlAttributes = null )
        {

            var form = bootstrap.FormLayout;
            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            var button = bootstrap.Html.LinkButton( label, buttonStyle, attributes, onClick );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> LinkButton<TModel>( this Bootstrap<TModel> bootstrap, string label, string action, string controller, object routeValues = null, ButtonStyle buttonStyle = ButtonStyle.Link, object htmlAttributes = null )
        {

            var form = bootstrap.FormLayout;
            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            var button = bootstrap.Html.LinkButton( label, buttonStyle, attributes, action, controller, routeValues );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> LinkButton<TModel>( this Bootstrap<TModel> bootstrap, string label, string action, string controller, string area, object routeValues = null, ButtonStyle buttonStyle = ButtonStyle.Link, object htmlAttributes = null )
        {

            var form = bootstrap.FormLayout;
            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            var button = bootstrap.Html.LinkButton( label, buttonStyle, attributes, action, controller, area, routeValues );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> Link<TModel>( this Bootstrap<TModel> bootstrap, string label, string href, object htmlAttributes = null )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            var button = bootstrap.Html.Link( label, attributes, href );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> Link<TModel>( this Bootstrap<TModel> bootstrap, string label, string action, string controller, object routeValues = null, object htmlAttributes = null )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            var button = bootstrap.Html.Link( label, attributes, action, controller, routeValues );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Bootstrap<TModel> Link<TModel>( this Bootstrap<TModel> bootstrap, string label, string action, string controller, string area, object routeValues = null, object htmlAttributes = null )
        {

            var attributes = Bootstrap.GetAttributes( htmlAttributes );
            var button = bootstrap.Html.Link( label, attributes, action, controller, area, routeValues );

            bootstrap.Append( bootstrap.WrapForInputButtonGroup( button.ToString() ) );

            return bootstrap;

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

        /// <summary>
        /// Gets the formstyle which may have been set by the Bootstrap.BeginForm extension
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="bootstrap"></param>
        /// <returns></returns>
        public static FormLayout GetFormLayout( this HtmlHelper html )
        {
            if( html.ViewContext.TempData.ContainsKey( "form-layout" ) == false )
            {
                return FormLayout.Vertical();
            }
            try
            {
                return (FormLayout)html.ViewContext.TempData[ "form-layout" ];
            }

            catch
            {
                return FormLayout.Vertical();
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
