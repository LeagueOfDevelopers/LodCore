using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
{
    public class ProjectView
    {
        public ProjectView(ProjectDto projectDto)
        {
            ProjectId = projectDto.ProjectId;
            Info = projectDto.Info;
            ProjectStatus = projectDto.ProjectStatus;
            LandingImage = new ImageView(projectDto.BigPhotoUri, projectDto.SmallPhotoUri);

            var screenshots = new List<ImageView>();
            projectDto.Screenshots.ForEach(i =>
            {
                if (i != null)
                    screenshots.Add(new ImageView(i.BigPhotoUri, i.SmallPhotoUri));
            });
            Screenshots = screenshots;
            //ProjectMemberships = projectDto.Developers;
        }

        public int ProjectId { get; }
        public string Name { get; }

        //public IEnumerable<ProjectType> ProjectTypes { get; set; }

        public string Info { get; }
        public ProjectStatus ProjectStatus { get; }
        public ImageView LandingImage { get; }
        public IEnumerable<ImageView> Screenshots { get; }
        //public IEnumerable<ProjectMembershipDto> ProjectMemberships { get; }
        
        //public virtual ISet<ProjectLink> Links { get; set; }
        //public virtual ISet<Uri> LinksToGithubRepositories { get; set; }
    }
}
