﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Walkies.Core.Configuration
{
    public class ConsoleSettings
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
            
        [JsonProperty( PropertyName = "show-ipad" )]
        public bool ShowLogForIPad { get; set; }

        [JsonProperty( PropertyName = "override-console-log" )]
        public bool OverrideConsoleLog { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
