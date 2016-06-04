using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walkies
{
    /// <summary>
    /// Settings used to help make image file validation quick & easy.
    /// </summary>
    public class ImageFileValidationSettings : FileValidationSettings
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        /// <summary>
        /// This has been made protected to try and reduce the need to create new instances of this object through regular means.
        /// The aim is to have properties for all the common file formats.
        /// </summary>
        protected ImageFileValidationSettings()
            : base()
        {

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Events

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
        /// <param name="maxWidth">The maximum width for the image</param>
        /// <param name="maxHeight">The maximum height for the image</param>
        public static ImageFileValidationSettings GetInstance( string[] validFormats, int fileSizeLimit, int maxWidth, int maxHeight )
        {
            return new ImageFileValidationSettings()
            {
                ValidFormats = validFormats,
                FileSizeLimit = fileSizeLimit,
                MaxWidth = maxWidth,
                MaxHeight = maxHeight,
                MinWidth = 0,
                MinHeight = 0
            };
        }

        /// <summary>
        /// A last resort if a property does not exist for the formats you require.
        /// </summary>
        /// <param name="validFormats">The file formats to test for. Leave out the '.'</param>
        /// <param name="fileSizeLimit">The maximum file size (in bytes) to check for</param>
        /// <param name="maxWidth">The maximum width for the image</param>
        /// <param name="maxHeight">The maximum height for the image</param>
        /// <param name="minWidth">The minimum width for the image</param>
        /// <param name="minHeight">The minimum height for the image</param>
        public static ImageFileValidationSettings GetInstance( string[] validFormats, int fileSizeLimit, int maxWidth, int maxHeight, int minWidth, int minHeight )
        {
            return new ImageFileValidationSettings()
            {
                ValidFormats = validFormats,
                FileSizeLimit = fileSizeLimit,
                MaxWidth = maxWidth,
                MaxHeight = maxHeight,
                MinWidth = minWidth,
                MinHeight = minHeight
            };
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        /// <summary>
        /// The maximum width for the image.
        /// </summary>
        public int MaxWidth { get; protected set; }

        /// <summary>
        /// The maximum height for the image.
        /// </summary>
        public int MaxHeight { get; protected set; }

        /// <summary>
        /// The minimum width for the image.
        /// </summary>
        public int MinWidth { get; protected set; }

        /// <summary>
        /// The minimum height for the image.
        /// </summary>
        public int MinHeight { get; protected set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Properties
        
        /// <summary>
        /// Image validation settings for a 1 MB image with dimensions of 1920x1080 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image1MBLimit_1920x1080
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 ),
                    MaxWidth = 1920,
                    MaxHeight = 1080
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 1 MB image with dimensions of 1280x720 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image1MBLimit_1280x720
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 ),
                    MaxWidth = 1280,
                    MaxHeight = 720
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 1 MB image with dimensions of 1024x768 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image1MBLimit_1024x768
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 ),
                    MaxWidth = 1280,
                    MaxHeight = 720
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 1 MB image with dimensions of 128x128 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image1MBLimit_128x128
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 ),
                    MaxWidth = 128,
                    MaxHeight = 128
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 1 MB image with dimensions of 64x64 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image1MBLimit_64x64
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 ),
                    MaxWidth = 64,
                    MaxHeight = 64
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 2 MB image with dimensions of 1920x1080 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image2MBLimit_1920x1080
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 2 ),
                    MaxWidth = 1920,
                    MaxHeight = 1080
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 2 MB image with dimensions of 1280x720 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image2MBLimit_1280x720
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 2 ),
                    MaxWidth = 1280,
                    MaxHeight = 720
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 2 MB image with dimensions of 1024x768 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image2MBLimit_1024x768
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 2 ),
                    MaxWidth = 1280,
                    MaxHeight = 720
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 2 MB image with dimensions of 128x128 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image2MBLimit_128x128
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 2 ),
                    MaxWidth = 128,
                    MaxHeight = 128
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 2 MB image with dimensions of 64x64 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image2MBLimit_64x64
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 2 ),
                    MaxWidth = 64,
                    MaxHeight = 64
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 5 MB image with dimensions of 1920x1080 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image5MBLimit_1920x1080
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 5 ),
                    MaxWidth = 1920,
                    MaxHeight = 1080
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 5 MB image with dimensions of 1280x720 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image5MBLimit_1280x720
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 5 ),
                    MaxWidth = 1280,
                    MaxHeight = 720
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 5 MB image with dimensions of 1024x768 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image5MBLimit_1024x768
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 5 ),
                    MaxWidth = 1280,
                    MaxHeight = 720
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 5 MB image with dimensions of 128x128 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image5MBLimit_128x128
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 5 ),
                    MaxWidth = 128,
                    MaxHeight = 128
                };
            }
        }

        /// <summary>
        /// Image validation settings for a 5 MB image with dimensions of 64x64 and of extensions:
        /// "png", "jpg", "jpeg", "gif"
        /// </summary>
        public static ImageFileValidationSettings Image5MBLimit_64x64
        {
            get
            {
                return new ImageFileValidationSettings()
                {
                    ValidFormats = supportedImageFormats,
                    FileSizeLimit = ( sizeof( byte ) * 1024 * 1024 * 5 ),
                    MaxWidth = 64,
                    MaxHeight = 64
                };
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
