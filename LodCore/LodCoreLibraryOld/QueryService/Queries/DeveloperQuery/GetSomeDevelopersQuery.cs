using System.Collections.Generic;
using System.Linq;
using LodCoreLibraryOld.QueryService.DTOs;
using LodCoreLibraryOld.QueryService.Views.DeveloperView;

namespace LodCoreLibraryOld.QueryService.Queries.DeveloperQuery
{
    public class GetSomeDevelopersQuery : IQuery<SomeDevelopersView>
    {
        public GetSomeDevelopersQuery(int offset, int count)
        {
            Offset = offset;
            Count = count;
            Sql = "SELECT * FROM accounts AS Account " +
                  "LEFT JOIN projectMemberships AS projMembership ON Account.userId = projMembership.developerId;";
        }

        public string Sql { get; }
        public int Offset { get; }
        public int Count { get; }

        public SomeDevelopersView FormResult(IEnumerable<AccountDto> rawResult)
        {
            var necessaryDevelopers = rawResult.Skip(Offset).Take(Count);
            return new SomeDevelopersView(necessaryDevelopers, rawResult.Count());
        }
    }
}