using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Core.Enumerations;
using Walkies.Database.JsonProperties;
using Newtonsoft.Json;

namespace Walkies.Database.Entities
{

    public partial class User 
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
        /// Occurs when the entity is materialised
        /// </summary>
        public void OnMaterialise()
        {
            if ( string.IsNullOrEmpty( this.PropertiesJson ) == false )
            {
                this.Properties = JsonConvert.DeserializeObject<UserProperties>( this.PropertiesJson );

                if ( this.Properties == null )
                {
                    this.Properties = new UserProperties();
                }
            }
            else
            {
                this.Properties = new UserProperties();
            }
        }

        /// <summary>
        /// Occurs before the entity is saved
        /// </summary>
        public void OnBeforeSave()
        {
            if ( Properties != null )
            {
                this.PropertiesJson = JsonConvert.SerializeObject( this.Properties );
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

        [EntityName]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        [NotMapped]
        public UserProperties Properties { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
