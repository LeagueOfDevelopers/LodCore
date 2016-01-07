using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Domain;

namespace FrontendServices.App_Data.AuthorizationPolicies
{
    public static class DevelopersPolicies
    {
        public static IEnumerable<Account> NonHiddenOnly(this IEnumerable<Account> accounts)
        {
            throw new NotImplementedException();
        }
    }
}