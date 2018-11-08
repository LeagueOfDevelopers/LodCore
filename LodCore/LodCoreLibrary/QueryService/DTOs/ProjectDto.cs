using LodCoreLibrary.Domain.ProjectManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.DTOs
{
    public class ProjectDto
    {
        public ProjectDto()
        {
        }

        public ProjectDto(Project project)
        {
            ProjectId = project.ProjectId;
            Name = project.Name;
            Info = project.Info;
            ProjectStatus = project.ProjectStatus;
            BigPhotoUri = project.LandingImage.BigPhotoUri.ToString();
            SmallPhotoUri = project.LandingImage.SmallPhotoUri.ToString();

            Screenshots = new HashSet<ImageDto>();
            project.Screenshots.ToList().ForEach(s => Screenshots.Add(new ImageDto(s, ProjectId)));

            Types = new HashSet<ProjectTypeDto>();
            project.ProjectTypes.ToList().ForEach(t => Types.Add(new ProjectTypeDto(t, ProjectId)));
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string BigPhotoUri { get; set; }
        public string SmallPhotoUri { get; set; }
        public string LinkName { get; set; }
        public Uri UriLink { get; set; }
        public ISet<ImageDto> Screenshots { get; set; }
        public ISet<ProjectMembershipDto> Developers { get; set; }
        public ISet<ProjectTypeDto> Types { get; set; }
        public ISet<ProjectLinkDto> Links { get; set; }
    }
}
