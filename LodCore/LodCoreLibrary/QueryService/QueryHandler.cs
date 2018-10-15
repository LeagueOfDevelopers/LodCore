using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.Queries;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LodCoreLibrary.QueryService.DTOs;

namespace LodCoreLibrary.QueryService
{
    public class QueryHandler : IQueryHandler
    {
        private IQueryDescriber _queryDescriber;
        private string _connectionString;

        public QueryHandler(IQueryDescriber queryDescriber, string connectionString)
        {
            _queryDescriber = queryDescriber;
            _connectionString = connectionString;
        }

        public dynamic Handle(IQuery query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return Handle((dynamic)query, connection);
            }
        }

        private IEnumerable<ProjectDto> Handle(AllProjectsQuery query, SqlConnection connection)
        {
            var sql = _queryDescriber.Describe(query);
            return connection.Query<ProjectDto>(sql).ToList();
        }
    }
}
