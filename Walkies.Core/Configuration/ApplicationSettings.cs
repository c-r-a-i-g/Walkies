using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;

namespace Walkies.Core.Configuration
{
    public class ApplicationSettings
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #if INTEGRATION
            public const string Environment = "integration";
        #elif STAGING
            public const string Environment = "staging";
        #elif RELEASE
            public const string Environment = "release";
        #else
            public const string Environment = "debug";
        #endif

        private static ApplicationSettings _appSettings = null;

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Attempts to load the server settings from the projects settings folder, or returns the default settings
        /// if none were found.
        /// </summary>
        /// <returns>A</returns>
        private static ApplicationSettings Load()
        {

            var path = string.Format( "~/content/_app-settings/{0}.json", Environment );
            var fileName = HttpContext.Current.Server.MapPath( path );

            Debug.Print( "Using {0} application settings", Environment.ToUpper() );
            Debug.Print( "Loading application settings from: {0}", fileName );

            // If the default or provided filename exists, attempt to load the settings
            if( File.Exists( fileName ) )
            {
                var content = File.ReadAllText( fileName );
                var settings = JsonConvert.DeserializeObject<ApplicationSettings>( content );
                Debug.Print( "Application settings were loaded successfully" );
                return settings;
            }

            throw new FileNotFoundException( "The application configuration file was not found", fileName );

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

        [JsonProperty( PropertyName = "application-client-id" )]
        public Guid ApplicationClientId { get; set; }

        [JsonProperty( PropertyName = "session-timeout-in-seconds" )]
        public int SessionTimeout { get; set; }

        [JsonProperty( PropertyName = "deal-cache-timeout-in-seconds" )]
        public int DealCacheTimeout { get; set; }

        [JsonProperty( PropertyName = "password-settings" )]
        public PasswordGeneratorSettings Passwords { get; set; }

        [JsonProperty( PropertyName = "email-settings" )]
        public EmailSettings Email { get; set; }

        [JsonProperty( PropertyName = "sms-settings" )]
        public SmsSettings Sms { get; set; }

        [JsonProperty( PropertyName = "console-settings" )]
        public ConsoleSettings Console { get; set; }
        
        [JsonProperty( "login-settings" )]
        public LoginSettings Login { get; set; }

        [JsonProperty( "integration-hub" )]
        public IntegrationHubSettings IntegrationHub { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        /// <summary>
        /// Gets the application settings for the current theme
        /// </summary>
        public static ApplicationSettings Current
        {
            get
            {

                if( _appSettings == null )
                {
                    _appSettings = ApplicationSettings.Load();
                }
                return _appSettings;
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
