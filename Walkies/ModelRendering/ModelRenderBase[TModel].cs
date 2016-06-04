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
    public abstract class ModelRenderBase<TModel> : ModelRenderBase where TModel : IModelRenderResult, new()
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
        protected ModelRenderBase( string viewDirectory = "" ) : base( viewDirectory )
        {
            this.Result = new TModel();
        }

        /// <summary>
        /// Creates a new model that can render a view
        /// </summary>
        /// <param name="viewName">The name of the view to render</param>
        public ModelRenderBase( string viewName, string viewDirectory = "" ) : base( viewName, viewDirectory )
        {
            this.Result = new TModel();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Renders the model using its view and returns the result as an object with Html property, plus
        /// any other properties that have been filled by the render process
        /// </summary>
        public new TModel Render()
        {
            return CreateViewRenderService().Render<TModel>( this );
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

        public TModel Result { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}

