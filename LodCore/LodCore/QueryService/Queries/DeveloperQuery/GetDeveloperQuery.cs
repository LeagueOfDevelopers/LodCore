using LodCore.QueryService.Views.DeveloperView;

namespace LodCore.QueryService.Queries.DeveloperQuery
{
    public class GetDeveloperQuery : IQuery<FullDeveloperView>
    {
        public GetDeveloperQuery(int developerId)
        {
            DeveloperId = developerId;
            Sql = "SELECT * FROM accounts AS Account " +
                  "LEFT JOIN projectmembership AS projMembership ON Account.userId = projMembership.developerId " +
                  "LEFT JOIN projects AS Project ON projMembership.projectId = project.projectId " +
                  $"WHERE Account.userId = {DeveloperId};";
        }

        public int DeveloperId { get; }
        public string Sql { get; }
    }
}