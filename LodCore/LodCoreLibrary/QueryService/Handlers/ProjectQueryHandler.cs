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
    public class ProjectQueryHandler : IQueryHandler<AllProjectsQuery, AllProjectsView>, 
        IQueryHandler<GetProjectQuery, FullProjectView>
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
            List<ProjectDto> result;

            using (var connection = new SqlConnection(_connectionString))
            {
                result = connection.Query<ProjectDto>(query.Sql).ToList();
            }

            return query.FormResult(result);
        }

        public FullProjectView Handle(GetProjectQuery query)
        {
            List<ProjectDto> allProjects;

            using (var connection = new SqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, ProjectDto>();

                allProjects = connection.Query<ProjectDto, ImageDto, ProjectMembershipDto, ProjectLinkDto, ProjectTypeDto, ProjectDto>(query.Sql,
                    (project, screenshot, projectMembership, projectLink, projectType) =>
                    {
                        ProjectDto projectEntry;

                        if (!resultDictionary.TryGetValue(project.ProjectId, out projectEntry))
                        {
                            projectEntry = project;
                            projectEntry.Screenshots = new List<ImageDto>();
                            projectEntry.Developers = new List<ProjectMembershipDto>();
                            projectEntry.Links = new List<ProjectLinkDto>();
                            projectEntry.Types = new List<ProjectTypeDto>();
                            resultDictionary.Add(projectEntry.ProjectId, projectEntry);
                        }
                                                
                        if (screenshot != null && !projectEntry.Screenshots.Any(s => s.BigPhotoUri == screenshot.BigPhotoUri))
                            projectEntry.Screenshots.Add(screenshot);

                        if (projectMembership != null && !projectEntry.Developers.Any(d => d.MembershipId == projectMembership.MembershipId))
                            projectEntry.Developers.Add(projectMembership);

                        if (projectLink != null && !projectEntry.Links.Any(l => l.Uri == projectLink.Uri))
                            projectEntry.Links.Add(projectLink);

                        if (projectType != null && !projectEntry.Types.Any(t => t.Type == projectType.Type))
                            projectEntry.Types.Add(projectType);

                        return projectEntry;
                    }, splitOn: "projectId,membershipId,projectId,projectId").Distinct().ToList();
            }

            var result = allProjects.Find(p => p.ProjectId == query.ProjectId);

            return new FullProjectView(result);
        }
    }
}
