using LodCore.QueryService.Views.DeveloperView;

namespace LodCore.QueryService.Queries.DeveloperQuery
{
    public class SearchDevelopersQuery : IQuery<SearchDevelopersView>
    {
        public SearchDevelopersQuery(string searchString)
        {
            SearchString = searchString;
            Sql = "SELECT * FROM accounts AS Account " +
                  "LEFT JOIN projectmembership ON Account.userId = projectmembership.developerId ";
        }

        public string SearchString { get; }
        public string Sql { get; }
    }
}