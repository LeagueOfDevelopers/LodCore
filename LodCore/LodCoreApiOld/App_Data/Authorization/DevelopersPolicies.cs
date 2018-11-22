using LodCoreLibraryOld.Domain.UserManagement;
using System;
using System.Collections.Generic;

namespace LodCoreApiOld.App_Data.Authorization
{
    public static class DevelopersPolicies
    {
        public static IEnumerable<Account> NonHiddenOnly(this IEnumerable<Account> accounts)
        {
            throw new NotImplementedException();
        }
    }
}