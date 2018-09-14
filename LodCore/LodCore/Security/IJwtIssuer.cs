using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LodCore.Security
{
    public interface IJwtIssuer
    {
        string IssueJwt(string role, int id);
    }
}
