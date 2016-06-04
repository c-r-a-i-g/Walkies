using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;

using NReco;
using NReco.PdfGenerator;

using Walkies.Framework.BaseClasses;

namespace Walkies.Framework.PdfGenerator
{
    public static class Pdf
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// Creates a PDF filestreamresult from the specified view
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="orientation"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStreamResult FromView( WebControllerBase controller, string viewName, Orientation orientation, int javascriptDelay, string fileName = "" )
        {
            return FromView( controller, viewName, null, orientation, javascriptDelay, fileName );
        }

        /// <summary>
        /// Creates a PDF filestreamresult from the specified view and model
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="orientation"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStreamResult FromView( WebControllerBase controller, string viewName, object model, Orientation orientation, int javascriptDelay, string fileName = "" )
        {

            var converter = CreateConverter( orientation, javascriptDelay );
            var html = controller.RenderToString( viewName, model );

            if( string.IsNullOrEmpty( html ) ) return null;

            var bytes = converter.GeneratePdf( html );
            var result = new FileStreamResult( new MemoryStream( bytes ), "application/pdf" );
            result.FileDownloadName = fileName;
            return result;

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Creates a Html to Pdf converter to transform the html
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        private static HtmlToPdfConverter CreateConverter( Orientation orientation, int javascriptDelay )
        {

            var converter = new HtmlToPdfConverter();
            converter.CustomWkHtmlPageArgs = PdfFlags.IgnoreLoadErrors
                                           + PdfFlags.IgnoreMediaErrors;

            if( javascriptDelay > 0 )
            {
                converter.CustomWkHtmlPageArgs += string.Format( PdfFlags.JavascriptDelay, javascriptDelay );
            }

            converter.Orientation = orientation == Orientation.Portrait ? PageOrientation.Portrait : PageOrientation.Landscape;
            converter.Margins = new PageMargins { Top = 0, Bottom = 0, Left = 0, Right = 0 };

            return converter;

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
