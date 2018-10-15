using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LodCoreLibrary.QueryService.Queries;

namespace LodCoreLibrary.QueryService
{
    public class QueryDescriber : IQueryDescriber
    {
        public string Describe(IQuery query)
        {
            switch (query)
            {
                case AllProjectsQuery knownQuery:
                    return "SELECT * FROM projects";

                case GetProjectQuery knownQuery:
                    return $"SELECT * FROM Projects WHERE Id = {knownQuery.ProjectId}";

                default:
                    throw new InvalidOperationException($"Unknown event {query.GetType()} to describe");
            }
        }
    }
}
