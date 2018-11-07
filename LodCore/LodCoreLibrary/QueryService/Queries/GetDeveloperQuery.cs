using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries
{
    public class GetDeveloperQuery : IQuery<FullDeveloperView>
    {
        public GetDeveloperQuery(int developerId, string sql)
        {
            DeveloperId = developerId;
            Sql = "SELECT * FROM accounts AS Account " +
                "LEFT JOIN projectMemberships AS projMembership ON Account.userId = projMembership.developerId" +
                "LEFT JOIN projects AS Project ON projMembership.projectId = project.projectId" +
                $"WHERE Account.userId = {DeveloperId};";
        }

        public int DeveloperId { get; }
        public string Sql { get; }
    }
}
