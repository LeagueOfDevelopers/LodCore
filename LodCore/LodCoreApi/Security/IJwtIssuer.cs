using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LodCoreApi.Security
{
    public interface IJwtIssuer
    {
        string IssueJwt(string role, int id);
    }
}
