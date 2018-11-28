using LodCore.QueryService.Views.DeveloperView;
using System;
using System.Collections.Generic;
using System.Text;

namespace LodCore.QueryService.Queries.DeveloperQuery
{
    public class SearchDevelopersQuery : IQuery<SearchDevelopersView>
    {
        public SearchDevelopersQuery(string searchString)
        {
            SearchString = searchString;
            Sql = $"SELECT * FROM accounts AS Account LEFT JOIN projectMemberships AS projMembership " +
                $"ON Account.userId = projMembership.developerId WHERE firstname LIKE '%{searchString}%' " +
                $"OR lastname LIKE '%{searchString}%' OR specialization LIKE '%{searchString}%'";
        }

        public string SearchString { get; }
        public string Sql { get; }
    }
}
