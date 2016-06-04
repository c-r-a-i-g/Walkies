#if !PLATFORM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public class SaveState
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

        public static SaveState Success( dynamic key )
        {
            var result = SaveState.Successful;
            result.Key = key;
            return result;
        }

        public static SaveState Success( dynamic entity, dynamic key )
        {
            var result = SaveState.Successful;
            result.Key = key;
            result.Entity = entity;
            return result;
        }

        public static SaveState Successful
        {
            get
            {
                return new SaveState()
                {
                    IsError = false,
                    IsSuccessful = true,
                    IsAlreadyExists = false,
                    IsNotRequired = false
                };
            }
        }

        public static SaveState NotRequired
        {
            get
            {
                return  new SaveState()
                {
                    IsError = false,
                    IsSuccessful = true,
                    IsAlreadyExists = false,
                    IsNotRequired = true
                };
            }
        }

        public static SaveState AlreadyExists
        {
            get
            {
                return new SaveState()
                {
                    IsError = false,
                    IsSuccessful = false,
                    IsAlreadyExists = true,
                    IsNotRequired = false
                };
            }
        }

        public static SaveState Error
        {
            get
            {
                return new SaveState()
                {
                    IsError = true,
                    IsSuccessful = false,
                    IsAlreadyExists = false,
                    IsNotRequired = false
                };
            }
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public bool IsSuccessful { get; set; }
        public bool IsAlreadyExists { get; set; }
        public bool IsError { get; set; }
        public bool IsNotRequired { get; set; }
        //public int RecordId { get; set; }
        //public Guid RecordGuid { get; set; }
        public dynamic Key { get; set; }
        public dynamic Data { get; set; }
        public dynamic Entity { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
#endif