﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using LodCore.Domain.ProjectManagment;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Queries.ProjectQuery;
using LodCore.QueryService.Views.ProjectView;
using MySql.Data.MySqlClient;

namespace LodCore.QueryService.Handlers
{
    public class ProjectQueryHandler :
        IQueryHandler<GetSomeProjectsQuery, SomeProjectsView>,
        IQueryHandler<GetProjectQuery, FullProjectView>,
        IQueryHandler<AllProjectsQuery, AllProjectsView>
    {
        private readonly string _connectionString;

        public ProjectQueryHandler(string connectionString)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _connectionString = connectionString;
        }

        public AllProjectsView Handle(AllProjectsQuery query)
        {
            List<ProjectDto> result;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, ProjectDto>();

                result = connection.Query<ProjectDto>(query.Sql).ToList();
            }

            return query.FormResult(result);
        }

        public FullProjectView Handle(GetProjectQuery query)
        {
            FullProjectView result = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var req = connection
                    .Query<FullProjectView, ImageView, ImageView, ProjectType, ProjectMembershipView, ProjectLinkView,
                        FullProjectView>(
                        query.Sql, (full, land, screen, type, member, link) =>
                        {
                            if (result == null)
                            {
                                result = full;
                                result.LandingImage = land;
                            }

                            if (screen != null && !result.Screenshots.Any(el => el.Equals(screen)))
                                result.Screenshots.Add(screen);

                            if (member != null && !result.ProjectMemberships.Contains(member))
                                result.ProjectMemberships.Add(member);

                            if (!result.ProjectTypes.Contains(type)) result.ProjectTypes.Add(type);

                            if (link != null && !result.Links.Contains(link)) result.Links.Add(link);

                            return full;
                        }, new {query.ProjectId}, splitOn: "BigPhotoUri,BigPhotoUri,Type,DeveloperId,Name");

                return result;
            }
        }

        public SomeProjectsView Handle(GetSomeProjectsQuery query)
        {
            List<ProjectDto> result;

            using (var connection = new MySqlConnection(_connectionString))
            {
                var resultDictionary = new Dictionary<int, ProjectDto>();

                result = connection.Query<ProjectDto, ProjectTypeDto, ProjectDto>(query.Sql,
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
                    }, splitOn: "project_key").Distinct().ToList();
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