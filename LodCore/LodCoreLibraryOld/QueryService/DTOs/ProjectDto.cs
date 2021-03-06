﻿using System.Collections.Generic;
using LodCoreLibraryOld.Domain.ProjectManagment;

namespace LodCoreLibraryOld.QueryService.DTOs
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string BigPhotoUri { get; set; }
        public string SmallPhotoUri { get; set; }
        public string LinkName { get; set; }
        public string UriLink { get; set; }
        public ISet<ImageDto> Screenshots { get; set; }
        public ISet<ProjectMembershipDto> Developers { get; set; }
        public ISet<ProjectTypeDto> Types { get; set; }
        public ISet<ProjectLinkDto> Links { get; set; }
    }
}