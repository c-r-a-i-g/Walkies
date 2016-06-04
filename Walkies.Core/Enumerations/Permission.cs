using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Core.Enumerations
{

    /* NOTES:
     * 
     * - Administration* permissions MUST be given an ordinal < 100.
     * 
     * - Management* MUST be given an ordinal >= 100 && < 1000.
     * 
     * - General permissions MUST be given an ordinal >= 1000 && < 10000.
     *
     * - Access control permissions MUST be given an ordinal >= 10000.
     */
    public enum Permission
    {

        /* Administration ------------------------ */
        [Description( "Administrator" )]
        Administrator = 1,

        [Description( "Access Error Logs" )]
        AccessErrorLogs = 10,


        /* Management ---------------------------- */
        [Description( "Manage Clients" )]
        ManageClients = 100,

        [Description( "Manage Dealerships" )]
        ManageDealerships = 101,

        [Description( "Manage Users" )]
        ManageUsers = 102,

        [Description( "Manage Roles" )]
        ManageRoles = 103,

        [Description( "Manage Domain Settings" )]
        ManageDomainSettings = 104,
        
        /* Management ---------------------------- */
        [Description( "Change Entity States" )]
        ChangeEntityStates = 1000,
        

        /* Access Control ------------------------ */
        [Description( "Access DealerCell Website" )]
        AccessDealerCellWebsite = 10000,

        [Description( "Access Integration Hub" )]
        AccessIntegrationHub = 10001,

        [Description( "Access WebApi" )]
        AccessAPI = 10002,

    }

}
