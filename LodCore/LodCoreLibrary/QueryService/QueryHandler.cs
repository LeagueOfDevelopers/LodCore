using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.Queries;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService
{
    public class QueryHandler
    {
        private readonly string _connectionString;

        public QueryHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Project> Handle(AllProjectsQuery query)
        {
            return query.Ask(_connectionString);
        }

        public Project Handle(GetProjectQuery query)
        {
            return query.Ask(_connectionString);
        }
    }
}
