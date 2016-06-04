using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Walkies.ViewRendering
{
    public abstract class ModelRenderBase : DynamicObject, IViewDataContainer
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// Create a model that can render a view, using the class name as the view name (excluding 'Model').
        /// </summary>
        /// <remarks>Used when defining strongly typed Email classes.</remarks>
        protected ModelRenderBase( string viewDirectory = "" )
        {
            this.ViewName = this.DeriveViewNameFromClassName();
            this.ViewDirectory = viewDirectory;
            this.ViewData = new ViewDataDictionary( this );
        }

        /// <summary>
        /// Creates a new model that can render a view
        /// </summary>
        /// <param name="viewName">The name of the view to render</param>
        public ModelRenderBase( string viewName, string viewDirectory = "" )
        {
            if( viewName == null ) throw new ArgumentNullException( "viewName" );
            if( string.IsNullOrWhiteSpace( viewName ) ) throw new ArgumentException( "View name cannot be empty.", "viewName" );
            this.ViewName = viewName;
            this.ViewDirectory = viewDirectory;
            this.ViewData = new ViewDataDictionary( this );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Renders the model using its view and returns the results as a string
        /// </summary>
        public string Render()
        {
            return CreateViewRenderService().Render( this );
        }

        /// <summary>
        /// Stores the given value into the <see cref="ViewData"/>.
        /// </summary>
        /// <param name="binder">Provides the name of the view data property.</param>
        /// <param name="value">The value to store.</param>
        /// <returns>Always returns true.</returns>
        public override bool TrySetMember( SetMemberBinder binder, object value )
        {
            ViewData[ binder.Name ] = value;
            return true;
        }

        /// <summary>
        /// Tries to get a stored value from <see cref="ViewData"/>.
        /// </summary>
        /// <param name="binder">Provides the name of the view data property.</param>
        /// <param name="result">If found, this is the view data property value.</param>
        /// <returns>True if the property was found, otherwise false.</returns>
        public override bool TryGetMember( GetMemberBinder binder, out object result )
        {
            return ViewData.TryGetValue( binder.Name, out result );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// A function that returns an instance of <see cref="IEmailService"/>.
        /// </summary>
        public static Func<IModelRenderService> CreateViewRenderService = () => new ModelRenderService();

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string DeriveViewNameFromClassName()
        {
            var viewName = GetType().Name;
            if( viewName.EndsWith( "Model" ) ) viewName = viewName.Substring( 0, viewName.Length - "Model".Length );
            return viewName;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        /// <summary>
        /// The name of the view containing the email template.
        /// </summary>
        public string ViewDirectory { get; set; }

        /// <summary>
        /// The name of the view containing the email template.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// The view data to pass to the view.
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}

