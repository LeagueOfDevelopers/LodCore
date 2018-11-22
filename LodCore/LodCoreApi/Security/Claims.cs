using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LodCoreApi.Security
{
    public class Claims
    {
        public const string IdClaim = "UserId";

        public static class Roles
        {
            public const string RoleClaim = "Role";
            public const string Admin = "Admin";
            public const string User = "User";
        }
    }
}
