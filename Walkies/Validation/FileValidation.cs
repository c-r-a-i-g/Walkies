using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Walkies
{
    public class FileValidation
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Validates whether or not the file provided by the user a valid.
        /// </summary>
        /// <param name="imageFile">The file posted by the user.</param>
        /// <param name="settings">The settings to use in determining if the posted file is valid</param>
        /// <returns>A validation result that contains a message for the reason why the file failed validation</returns>
        public static ModelValidationResult Validate( HttpPostedFileBase file, FileValidationSettings settings )
        {
            if ( file == null || file.ContentLength == 0 )
            {
                return new ModelValidationResult( false, "No file was provided" );
            }

            return ValidateFileFormatAndSize( file, settings ) ?? new ModelValidationResult( true );
        }

        /// <summary>
        /// Validates whether or not the image file provided by the user is valid.
        /// </summary>
        /// <param name="imageFile">The file posted by the user</param>
        /// <param name="settings">The settings to use in determining if the posted file is valid</param>
        /// <returns>A validation result that contains a message for the reason why the file failed validation</returns>
        public static ModelValidationResult Validate( HttpPostedFileBase file, ImageFileValidationSettings settings )
        {
            if ( file == null || file.ContentLength == 0 )
            {
                return new ModelValidationResult( false, "No image file was provided" );
            }

            ModelValidationResult formatAndSizeResult = ValidateFileFormatAndSize( file, settings );

            if ( formatAndSizeResult != null )
            {
                return formatAndSizeResult;
            }

            try
            {
                Image image = Image.FromStream( file.InputStream );

                bool maxWidthIncorrect = settings.MaxWidth > 0 && image.Width > settings.MaxWidth;
                bool maxHeightIncorrect = settings.MaxHeight > 0 && image.Height > settings.MaxHeight;
                bool minWidthIncorrect = image.Width < settings.MinWidth;
                bool minHeightIncorrect = image.Height < settings.MinHeight;

                if ( maxWidthIncorrect || maxHeightIncorrect || minWidthIncorrect || minHeightIncorrect )
                {
                    string message = "";
                    if ( settings.MinWidth > 0 || settings.MinHeight > 0 )
                    {
                        message = string.Format( "The dimensions for the image are incorrect: the minimum dimensions are {0}x{1} and the maximum dimensions are {2}x{3}", settings.MinWidth, settings.MinHeight, settings.MaxWidth, settings.MaxHeight );
                    }
                    else
                    {
                        message = string.Format( "The dimensions for the image are incorrect: the maximum dimensions are {0}x{1}", settings.MaxWidth, settings.MaxHeight );
                    }

                    return new ModelValidationResult( false, message );
                }
            }
            catch
            {
                return new ModelValidationResult( false, "The file provided could not be converted to a valid image" );
            }

            return new ModelValidationResult( true );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Checks to see if the file is of the correct format and file size.
        /// Returns null if successful.
        /// </summary>
        /// <param name="file">The file posted by the user</param>
        /// <param name="settings">The settings to use in determining if the posted file is valid</param>
        /// <returns>A ValidationResult if the file is invalid, otherwise null</returns>
        private static ModelValidationResult ValidateFileFormatAndSize( HttpPostedFileBase file, FileValidationSettings settings )
        {
            int fileSize = file.ContentLength;
            string extension = Path.GetExtension( file.FileName ).ToLower();

            if ( settings.ValidFormats.Any( s => s == extension ) == false )
            {
                // Create the list of formats that are supported.
                string supportedFormats = "";
                settings.ValidFormats.ToList().ForEach( s => supportedFormats += ", \"." + s + "\"" );
                supportedFormats = supportedFormats.Remove( 0, 2 );

                return new ModelValidationResult( false, "The file provided is not supported. You can upload files with the extensions: " + supportedFormats );
            }

            if ( settings.FileSizeLimit > 0 && settings.FileSizeLimit < fileSize )
            {
                string sizeLimitString = " bytes";
                decimal currentSize = settings.FileSizeLimit;
                string sizeLimitFormat = "{0}";

                // Convert the file size limit provided into something more readable.
                if ( currentSize > 1024 )
                {
                    currentSize /= 1024;
                    sizeLimitString = "KB";

                    if ( currentSize > 1024 )
                    {
                        currentSize /= 1024;
                        sizeLimitString = "MB";
                    }
                }

                if ( currentSize != (int)currentSize )
                {
                    sizeLimitFormat = "{0:1}";
                }

                return new ModelValidationResult( false, "No file provided is too large, please use a file that is less than " + currentSize.ToString( sizeLimitFormat ) + sizeLimitString );
            }

            return null;
        }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
