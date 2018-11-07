using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries
{
    public class AllDevelopersQuery : IQuery<AllDevelopersView>
    {
        public AllDevelopersQuery(string sql)
        {
            Sql = "SELECT * FROM accounts AS Account " +
                "LEFT JOIN projectMemberships AS projMembership ON Account.userId = projMembership.developerId;";
        }

        public string Sql { get; }
    }
}
