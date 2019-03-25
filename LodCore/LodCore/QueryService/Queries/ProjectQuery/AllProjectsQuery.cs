using System.Collections.Generic;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views.ProjectView;

namespace LodCore.QueryService.Queries.ProjectQuery
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