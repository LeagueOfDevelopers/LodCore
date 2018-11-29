using Dapper;
using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Views;
using LodCore.QueryService.Views.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Queries.ProjectQuery
{
    public class GetSomeProjectsQuery : IQuery<SomeProjectsView>
    {
        public GetSomeProjectsQuery(int offset, int count, int[] categories, bool isAuthenticatedCallingUser)
        {
            Offset = offset;
            Count = count;
            Categories = categories;
            IsAuthenticatedCallingUser = IsAuthenticatedCallingUser;

            SqlForSomeProjects = "SELECT * FROM projects AS Project LEFT JOIN projectTypes AS ProjectType " +
                "ON Project.ProjectId = ProjectType.project_key WHERE id IN(@categories) " +
                "AND IF(@isAuthenticatedCallingUser, ProjectStatus IN(0,1,2,3), ProjectStatus=1 OR ProjectStatus=3) " +
                "LIMIT @offset,@count; ";

            SqlForAllProjects = "SELECT * FROM projects;";
        }
        
        public int Offset { get; }
        public int Count { get; }
        public int[] Categories { get; }
        public bool IsAuthenticatedCallingUser { get; }
        public string SqlForSomeProjects { get; }
        public string SqlForAllProjects { get; }
    }
}
