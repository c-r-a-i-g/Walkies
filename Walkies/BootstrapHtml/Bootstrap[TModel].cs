using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Web;

using Walkies.BootstrapHtml.Renderers;
using System.Reflection;

namespace Walkies.BootstrapHtml
{
    public class Bootstrap<TModel> : Bootstrap
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        private StringBuilder _builder = new StringBuilder();
        private int _openTags = 0;
        private bool _isGrouping = false;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public Bootstrap( ViewContext viewContext, IViewDataContainer container, HtmlHelper<TModel> htmlHelper ) : this( viewContext, container, RouteTable.Routes, htmlHelper )
        {
        }

        public Bootstrap( ViewContext viewContext, IViewDataContainer container, RouteCollection routeCollection, HtmlHelper<TModel> htmlHelper ) : base( viewContext, container, routeCollection, htmlHelper )  
        {
            Html = htmlHelper;
            ViewData = new ViewDataDictionary<TModel>( container.ViewData );
            FormLayout = FormLayout.Vertical();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

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
        /// Gets the name of the model property identified by the expression
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName<TProperty>( HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression )
        {

            var name = ExpressionHelper.GetExpressionText( expression );
            name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName( name );
            name = name.ReplaceAny( new string[] { ".", "[", "]" }, "_" );

            return name;

        }

        /// <summary>
        /// Gets the value of the property identified by the expression
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object GetExpressionValue<TProperty>( Expression<Func<TModel, TProperty>> expression )
        {
            var type = typeof( TModel );
            var member = expression.Body as MemberExpression;
            var propInfo = member.Member as PropertyInfo;
            return propInfo.GetValue( this.ViewData.Model );
        }

        public override string ToString()
        {

            if( _openTags > 0 )
            {
                for( int i = 0; i < _openTags; i += 1 )
                {
                    _builder.Append( "</div>" );
                }
                _openTags = 0;
            }

            Html.ViewContext.Writer.Write( _builder.ToString() );
            _builder.Clear();
            _isGrouping = false;

            return ""; 

        }

        public Bootstrap<TModel> FormGroup()
        {
            _builder.Append( "<div class=\"form-group\">" );
            _openTags += 1;
            return this;
        }
        
        public Bootstrap<TModel> Inline()
        {
            _builder.Append( "<div class=\"form-inline\">" );
            _openTags += 1;
            return this;
        }

        public Bootstrap<TModel> LabelColumn()
        {
            _builder.Append( string.Format( "<div class=\"{0}\">", this.FormLayout.LabelClass ) );
            _openTags += 1;
            return this;
        }

        public Bootstrap<TModel> FieldColumn( bool inline = false )
        {
            var inlineClass = inline ? " form-inline" : "";
            _builder.Append( string.Format( "<div class=\"{0}{1}\">", this.FormLayout.FieldClass, inlineClass ) );
            _openTags += 1;
            return this;
        }

        public Bootstrap<TModel> Group()
        {
            _builder.Append( string.Format( "<div class=\"{0} form-inline\">", this.FormLayout.FieldClass ) );
            _builder.Append( "<div class=\"input-group\">" );
            _openTags += 2;
            _isGrouping = true;
            return this;
        }

        public Bootstrap<TModel> LayoutIndent()
        {

            if( this.FormLayout.FormStyle == FormStyle.Horizontal )
            {
                _builder.Append( string.Format( "<div class=\"{0}\">&nbsp;</div>", this.FormLayout.LabelClass ) );
            }
            return this;

        }

        internal Bootstrap<TModel> EndFormGroup()
        {
            _builder.Append( "</div>" );
            _openTags -= 1;
            return this;
        }

        public Bootstrap<TModel> Append( string html )
        {
            _builder.Append( html );
            return this;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public new ViewDataDictionary<TModel> ViewData { get; private set; }
        public new HtmlHelper<TModel> Html { get; private set; }

        public FormLayout FormLayout { get; internal set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        public bool IsGrouping
        {
            get
            {
                return _isGrouping;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
