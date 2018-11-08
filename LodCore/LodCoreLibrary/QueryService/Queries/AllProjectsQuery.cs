using LodCoreLibrary.QueryService.DTOs;
using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries
{
    public class AllProjectsQuery : IQuery<AllProjectsView>
    {
        public AllProjectsQuery()
        {
            Sql = "SELECT * FROM projects;";
        }

        public string Sql { get; }

        public AllProjectsView FormResult(List<ProjectDto> rawResult)
        {
            return new AllProjectsView(rawResult);
        }
    }
}
