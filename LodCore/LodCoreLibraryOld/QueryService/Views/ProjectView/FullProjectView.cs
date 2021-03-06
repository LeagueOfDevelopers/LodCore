﻿using System.Collections.Generic;
using System.Linq;
using LodCoreLibrary.QueryService.Views.ProjectView;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.QueryService.DTOs;

namespace LodCoreLibraryOld.QueryService.Views.ProjectView
{
    public class FullProjectView
    {
        public FullProjectView(ProjectDto projectDto)
        {
            ProjectId = projectDto.ProjectId;
            Name = projectDto.Name;
            Info = projectDto.Info;
            ProjectStatus = projectDto.ProjectStatus;
            LandingImage = new ImageView(projectDto.BigPhotoUri, projectDto.SmallPhotoUri);

            ProjectTypes = new List<ProjectType>();
            projectDto.Types.ToList().ForEach(t => ProjectTypes.Add(t.Type));

            ProjectMemberships = new List<ProjectMembershipView>();
            projectDto.Developers.ToList()
                .ForEach(d => ProjectMemberships.Add(new ProjectMembershipView(d.DeveloperId, d.Role)));

            Screenshots = new List<ImageView>();
            projectDto.Screenshots.ToList()
                .ForEach(s => Screenshots.Add(new ImageView(s.BigPhotoUri, s.SmallPhotoUri)));

            Links = new List<ProjectLinkView>();
            projectDto.Links.ToList().ForEach(l => Links.Add(new ProjectLinkView(l.Name, l.Uri)));
        }

        public int ProjectId { get; }
        public string Name { get; }
        public List<ProjectType> ProjectTypes { get; }
        public string Info { get; }
        public ProjectStatus ProjectStatus { get; }
        public ImageView LandingImage { get; }
        public List<ProjectMembershipView> ProjectMemberships { get; }
        public List<ImageView> Screenshots { get; }
        public List<ProjectLinkView> Links { get; set; }

        public bool IsInProgressOrDone()
        {
            return ProjectStatus == ProjectStatus.Done || ProjectStatus == ProjectStatus.InProgress;
        }
    }
}