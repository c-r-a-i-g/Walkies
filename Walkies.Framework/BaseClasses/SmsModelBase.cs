using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Postal;

using Walkies.Core.Configuration;

namespace Walkies.Framework.BaseClasses
{
    public class SmsModelBase : EmailModelBase
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public SmsModelBase( string viewName ) : base( viewName ) 
        { 
        }

        public SmsModelBase( string viewName, string mobileNumber, string firstName ) : base( viewName )
        {

            this.FirstName = firstName;
            this.FromAddress = ApplicationSettings.Current.Sms.FromAddress;

            // if the override flag is set, use the override mobile number - this should be set in all
            // environments except the production evironment
            if( ApplicationSettings.Current.Sms.OverrideSend )
            {
                mobileNumber = ApplicationSettings.Current.Sms.OverrideMobileNumber;
            }

            this.ToAddress = string.Format( ApplicationSettings.Current.Sms.ToAddressTemplate, mobileNumber );

        }

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

        public string FirstName { get; set; }
        public string MobileNumber { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
