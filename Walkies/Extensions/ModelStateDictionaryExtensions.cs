using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Walkies
{
    public static class ModelStateDictionaryExtensions
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
        /// Removes any errors on the specified field
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static ModelStateDictionary RemoveError( this ModelStateDictionary modelState, string fieldName )
        {
            if( modelState.ContainsKey( fieldName ) )
            {
                modelState[ fieldName ].Errors.Clear();
            }
            return modelState;
        }

        /// <summary>
        /// Removes any errors on the specified field
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static void Remove<TModel>( this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression )
        {
            string expressionText = ExpressionHelper.GetExpressionText( expression );
            foreach( var ms in modelState.ToArray() )
            {
                if( ms.Key.StartsWith( expressionText + "." ) || ms.Key == expressionText )
                {
                    modelState.Remove( ms );
                }
            }
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
