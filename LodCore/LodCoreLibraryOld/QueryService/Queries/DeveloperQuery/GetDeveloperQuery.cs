using LodCoreLibraryOld.QueryService.Views;
using LodCoreLibraryOld.QueryService.Views.DeveloperView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.Queries.DeveloperQuery
{
    public class GetDeveloperQuery : IQuery<FullDeveloperView>
    {
        public GetDeveloperQuery(int developerId)
        {
            DeveloperId = developerId;
            Sql = "SELECT * FROM accounts AS Account " +
                "LEFT JOIN projectMemberships AS projMembership ON Account.userId = projMembership.developerId " +
                "LEFT JOIN projects AS Project ON projMembership.projectId = project.projectId " +
                $"WHERE Account.userId = {DeveloperId};";
        }

        public int DeveloperId { get; }
        public string Sql { get; }
    }
}
