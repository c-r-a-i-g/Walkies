﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkies.Interfaces;

namespace Walkies
{

    [AttributeUsage( AttributeTargets.Field )]
    public class ClassAttribute : Attribute, IAttributeValue
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public ClassAttribute( string className ) 
        {
            this.Value = className;
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

        public string Value { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}