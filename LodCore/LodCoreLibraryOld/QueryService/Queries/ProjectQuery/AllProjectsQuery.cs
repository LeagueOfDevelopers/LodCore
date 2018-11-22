using LodCoreLibraryOld.QueryService.DTOs;
using LodCoreLibraryOld.QueryService.Views;
using LodCoreLibraryOld.QueryService.Views.ProjectView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.Queries.ProjectQuery
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
