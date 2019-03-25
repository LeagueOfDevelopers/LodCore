using System;
using System.Collections.Generic;
using LodCoreLibraryOld.Domain.UserManagement;

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