using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries
{
    public class AllDevelopersQuery
    {
        public AllDevelopersQuery(string sql)
        {
            Sql = sql;
        }

        public string Sql { get; }
    }
}
