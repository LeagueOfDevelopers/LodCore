using Dapper;
using LodCoreLibrary.QueryService.DTOs;
using LodCoreLibrary.QueryService.Queries;
using LodCoreLibrary.QueryService.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Handlers
{
    public class ProjectQueryHandler : IQueryHandler<AllProjectsQuery, AllProjectsView>
    {
        private string _connectionString;
        private IQueryDescriber _queryDescriber;

        public ProjectQueryHandler(string connectionString, IQueryDescriber queryDescriber)
        {
            _connectionString = connectionString;
            _queryDescriber = queryDescriber;
        }

        public AllProjectsView Handle(AllProjectsQuery query)
        {
            string sql = _queryDescriber.Describe(query);
            List<ProjectDto> result;

            using (var connection = new SqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, ProjectDto>();

                result = connection.Query<ProjectDto, ImageDto, ProjectDto>(sql, 
                    (project, screenshot) =>
                    {
                        ProjectDto projectEntry;

                        if (!resultDictionary.TryGetValue(project.ProjectId, out projectEntry))
                        {
                            projectEntry = project;
                            projectEntry.Screenshots = new List<ImageDto>();
                            resultDictionary.Add(projectEntry.ProjectId, projectEntry);
                        }

                        projectEntry.Screenshots.Add(screenshot);
                        
                        return projectEntry;
                    }, splitOn: "screenshotId").Distinct().ToList();
            }

            return query.FormResult(result);
        }
    }
}
