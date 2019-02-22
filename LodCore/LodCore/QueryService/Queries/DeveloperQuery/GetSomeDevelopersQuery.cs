using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views;
using LodCore.QueryService.Views.DeveloperView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Queries.DeveloperQuery
{
    public class GetSomeDevelopersQuery : IQuery<SomeDevelopersView>
    {
        public GetSomeDevelopersQuery(int offset, int count)
        {
            Offset = offset;
            Count = count;
            Sql = "SELECT * FROM accounts AS Account " +
                "LEFT JOIN projectMembership AS projMembership ON Account.userId = projMembership.developerId;";
        }

        public string Sql { get; }
        public int Offset { get; }
        public int Count { get; }

        public SomeDevelopersView FormResult(IEnumerable<AccountDto> rawResult)
        {
            var visableDevelopers = rawResult
                .Where(dev => !dev.IsHidden);

            int countVisableDevelopers = visableDevelopers.Count();
            visableDevelopers = visableDevelopers
                .Skip(Offset)
                .Take(Count);
                
            return new SomeDevelopersView(visableDevelopers.ToList(), countVisableDevelopers);
        }
    }
}
