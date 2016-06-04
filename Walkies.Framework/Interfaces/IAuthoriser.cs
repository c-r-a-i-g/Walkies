using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Walkies.Framework.Interfaces
{

    /// <summary>
    /// Classes that implement this interface can be used to provide authorisation to database
    /// entities through the controller action, using the AuthoriseWithAttribute
    /// </summary>
    public interface IAuthorisor
    {
        bool CanAccess( AuthorizationContext context );

    }

}
