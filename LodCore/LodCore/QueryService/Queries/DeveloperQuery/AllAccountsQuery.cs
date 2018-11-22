using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views.DeveloperView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
