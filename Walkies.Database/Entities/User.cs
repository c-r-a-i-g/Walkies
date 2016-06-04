using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Walkies.Database.Interfaces;

namespace Walkies.Database.Entities
{

    [Table( "User" )]
    public partial class User : IActive, IOnMaterialise, IOnBeforeSave
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public User()
        {
            this.UserId = Guid.NewGuid();
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

        [Required, Key]
        public Guid UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int InvalidLoginAttempts { get; set; }

        [Required]
        public bool IsLocked { get; set; }
        
        public string PropertiesJson { get; set; }

        [Required]
        public bool IsActive { get; set; }
        
        public DateTime? DateAgreedTerms { get; set; }
        public DateTime? DateLastLogin { get; set; }

        [DatabaseGenerated( DatabaseGeneratedOption.Computed )]
        public DateTime DateCreated { get; set; }


        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Navigation Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
