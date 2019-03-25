using System.Collections.Generic;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views.DeveloperView;

namespace LodCore.QueryService.Queries.DeveloperQuery
{
    public class AllAccountsQuery : IQuery<AllAccountsView>
    {
        public AllAccountsQuery()
        {
            Sql = "SELECT * FROM accounts;";
        }

        public string Sql { get; }

        public AllAccountsView FormResult(List<AccountDto> rawResult)
        {
            return new AllAccountsView(rawResult);
        }
    }
}