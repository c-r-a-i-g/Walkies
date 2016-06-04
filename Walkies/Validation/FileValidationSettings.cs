using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Walkies
{
    public class FileValidationSettings
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        /// <summary>
        /// The most common file formats that we have chosen to support for images.
        /// </summary>
        protected internal static string[] supportedImageFormats = new string[] { "png", "jpg", "jpeg", "gif" };

        /// <summary>
        /// The most common files formats that we have chosen to support for pdfs.
        /// </summary>
        protected internal static string[] supportedPdfFormats = new string[] { "pdf" };

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// This has been made protected to try and reduce the need to create new instances of this object through regular means.
        /// The aim is to have properties for all the common file formats.
        /// </summary>
        protected FileValidationSettings()
        {

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        /// <summary>
        /// A last resort if a property does not exist for the formats you require.
        /// </summary>
        /// <param name="validFormats">The file formats to test for. Leave out the '.'</param>
        /// <param name="fileSizeLimit">The maximum file size (in bytes) to check for</param>
        public static FileValidationSettings GetInstance( string[] validFormats, int fileSizeLimit )
        {
            if ( validFormats == null || validFormats.Length == 0 )
            {
                throw new Exception( "Invalid file formats provided" );
            }

            return new FileValidationSettings()
            {
                ValidFormats = validFormats,
                FileSizeLimit = fileSizeLimit
            };
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        /// <summary>
        /// The valid file formats.
        /// </summary>
        public string[] ValidFormats { get; protected set; }

        /// <summary>
        /// The image size limit ( in bytes ).
        /// Use 0 to represent unlimited.
        /// </summary>
        public int FileSizeLimit { get; protected set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Properties

        /// <summary>
        /// Image validation settings for a 1 MB image of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static FileValidationSettings Image1MBLimit
        {
            get
            {
                return new FileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 )
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 2 MB image of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static FileValidationSettings Image2MBLimit
        {
            get
            {
                return new FileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 2 )
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 5 MB image of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static FileValidationSettings Image5MBLimit
        {
            get
            {
                return new FileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 5 )
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 1 MB pdf
        /// </summary>
        public static FileValidationSettings Pdf1MBLimit
        {
            get
            {
                return new FileValidationSettings()
                {
                    ValidFormats = supportedPdfFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 )
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 2 MB pdf
        /// </summary>
        public static FileValidationSettings Pdf2MBLimit
        {
            get
            {
                return new FileValidationSettings()
                {
                    ValidFormats = supportedPdfFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 2 )
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 5 MB pdf
        /// </summary>
        public static FileValidationSettings Pdf5MBLimit
        {
            get
            {
                return new FileValidationSettings()
                {
                    ValidFormats = supportedPdfFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 5 )
                };
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
