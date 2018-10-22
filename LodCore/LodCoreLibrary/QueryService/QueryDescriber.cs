using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LodCoreLibrary.QueryService.Queries;
using LodCoreLibrary.QueryService.Views;

namespace LodCoreLibrary.QueryService
{
    public class QueryDescriber : IQueryDescriber
    {
        public string Describe(AllProjectsQuery query)
        {
            return "SELECT * FROM projects AS A FULL JOIN screenshots AS B ON A.projectId = B.projectId;";
        }

        public string Describe(GetProjectQuery query)
        {
            return $"SELECT * FROM Projects WHERE Id = {query.ProjectId}";
        }
    }
}
