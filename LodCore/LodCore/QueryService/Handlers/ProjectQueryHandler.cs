﻿using Dapper;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Queries;
using LodCore.QueryService.Queries.ProjectQuery;
using LodCore.QueryService.Views;
using LodCore.QueryService.Views.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LodCore.QueryService.Handlers
{
    public class ProjectQueryHandler : 
        IQueryHandler<GetSomeProjectsQuery, SomeProjectsView>, 
        IQueryHandler<GetProjectQuery, FullProjectView>,
        IQueryHandler<AllProjectsQuery, AllProjectsView>
    {
        private string _connectionString;

        public ProjectQueryHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SomeProjectsView Handle(GetSomeProjectsQuery query)
        {
            List<ProjectDto> result;
            int projectsCount;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, ProjectDto>();

                result = connection.Query<ProjectDto, ProjectTypeDto, ProjectDto>(query.SqlForSomeProjects,
                    (project, projectType) =>
                    {
                        ProjectDto projectEntry;

                        if (!resultDictionary.TryGetValue(project.ProjectId, out projectEntry))
                        {
                            projectEntry = project;
                            projectEntry.Types = new HashSet<ProjectTypeDto>();
                            resultDictionary.Add(projectEntry.ProjectId, projectEntry);
                        }
                        
                        return projectEntry;
                    }, new { query.Categories, query.IsAuthenticatedCallingUser, query.Offset, query.Count}, 
                    splitOn: "project_key").Distinct().ToList();

                projectsCount = connection.Query(query.SqlForAllProjects).Count();
            }

            return new SomeProjectsView(result, projectsCount);
        }

        public FullProjectView Handle(GetProjectQuery query)
        {
            ProjectDto result;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, ProjectDto>();

                result = connection.Query<ProjectDto, ImageDto, ProjectMembershipDto, ProjectTypeDto, ProjectDto>(query.Sql,
                    (project, screenshot, projectMembership, projectType) =>
                    {
                        ProjectDto projectEntry;

                        if (!resultDictionary.TryGetValue(project.ProjectId, out projectEntry))
                        {
                            projectEntry = project;
                            projectEntry.Screenshots = new HashSet<ImageDto>();
                            projectEntry.Developers = new HashSet<ProjectMembershipDto>();
                            projectEntry.Links = new HashSet<ProjectLinkDto>();
                            projectEntry.Types = new HashSet<ProjectTypeDto>();
                            resultDictionary.Add(projectEntry.ProjectId, projectEntry);
                        }
                                                
                        if (screenshot != null && !projectEntry.Screenshots.Any(s => s.BigPhotoUri == screenshot.BigPhotoUri))
                            projectEntry.Screenshots.Add(screenshot);

                        if (projectMembership != null && !projectEntry.Developers.Any(d => d.MembershipId == projectMembership.MembershipId))
                            projectEntry.Developers.Add(projectMembership);

                        if (projectType != null && !projectEntry.Types.Any(t => t.Id == projectType.Id))
                            projectEntry.Types.Add(projectType);

                        return projectEntry;
                    }, splitOn: "project_key,membershipId,project_key").Distinct().ToList().First();
            }
            
            return new FullProjectView(result);
        }

        public AllProjectsView Handle(AllProjectsQuery query)
        {
            List<ProjectDto> result;

            using (var connection = new SqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, ProjectDto>();

                result = connection.Query<ProjectDto>(query.Sql).ToList();
            }

            return query.FormResult(result);
        }

        // Future mapping-method
        /*
        private Func<ProjectDto, ImageDto, ProjectMembershipDto, ProjectLinkDto, ProjectTypeDto, ProjectDto>
            _mapFullProjectInfoFunc(ProjectDto project, ImageDto image, ProjectMembershipDto projectMembership,
            ProjectLinkDto projectLink, ProjectTypeDto projectType) = 

        private ProjectDto MapFullProjectInfoMethod(ProjectDto project, ImageDto screenshot, ProjectMembershipDto projectMembership,
            ProjectLinkDto projectLink, ProjectTypeDto projectType)
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
        }
        */
    }
}
