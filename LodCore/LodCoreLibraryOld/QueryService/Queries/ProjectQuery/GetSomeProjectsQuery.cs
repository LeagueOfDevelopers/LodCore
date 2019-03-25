using System.Collections.Generic;
using System.Linq;
using LodCoreLibraryOld.QueryService.DTOs;
using LodCoreLibraryOld.QueryService.Views.ProjectView;

namespace LodCoreLibraryOld.QueryService.Queries.ProjectQuery
{
    public class GetSomeProjectsQuery : IQuery<SomeProjectsView>
    {
        public GetSomeProjectsQuery(int offset, int count, int[] categories)
        {
            Offset = offset;
            Count = count;
            Categories = categories;

            Sql = "SELECT * FROM projects AS Project LEFT JOIN projectTypes AS ProjectType " +
                  $"ON Project.projectid = ProjectType.projectId WHERE type IN({string.Join(",", Categories)});";
        }

        public int Offset { get; }
        public int Count { get; }
        public int[] Categories { get; }
        public string Sql { get; }

        public SomeProjectsView FormResult(List<ProjectDto> rawResult)
        {
            var necessaryProjects = rawResult.Skip(Offset).Take(Count);
            return new SomeProjectsView(necessaryProjects, rawResult.Count());
        }
    }
}