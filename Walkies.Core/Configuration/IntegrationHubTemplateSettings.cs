using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using Walkies.Core.Interfaces;

namespace Walkies.Core.Configuration
{
    public class TemplateSettings : IIntegrationHubTemplateDefinition
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

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        [JsonProperty( "key" )]
        public string Key { get; set; }

        [JsonProperty( "path" )]
        public string Path { get; set; }

        [JsonProperty( "title" )]
        public string Title { get; set; }

        [JsonProperty( "type" )]
        public string Type { get; set; }

        [JsonProperty( "script" )]
        public string Script { get; set; }

        [JsonProperty( "sources" )]
        public List<string> Sources { get; set; }

        [JsonProperty( "resource" )]
        public string Resource { get; set; }

        [JsonProperty( "enabled" )]
        public bool IsEnabled { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties


        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}