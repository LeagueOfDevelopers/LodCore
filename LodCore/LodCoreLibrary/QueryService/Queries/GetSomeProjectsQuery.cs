using Dapper;
using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.DTOs;
using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Queries
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

        public SomeProjectsView FormResult(List<ProjectDto> rawResult)
        {
            var necessaryProjects = rawResult.Skip(Offset).Take(Count);
            return new SomeProjectsView(necessaryProjects);
        }

        public int Offset { get; }
        public int Count { get; }
        public int[] Categories { get; }
        public string Sql { get; }
    }
}
