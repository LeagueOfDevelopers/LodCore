using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.Views
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

            ProjectTypes = new List<int>();
            projectDto.Types.ForEach(t => ProjectTypes.Add(t.Type));

            ProjectMemberships = new List<ProjectMembershipView>();
            projectDto.Developers.ForEach(d => ProjectMemberships.Add(new ProjectMembershipView(d.DeveloperId, d.Role)));

            Screenshots = new List<ImageView>();
            projectDto.Screenshots.ForEach(s => Screenshots.Add(new ImageView(s.BigPhotoUri, s.SmallPhotoUri)));

            Links = new List<ProjectLinkView>();
            projectDto.Links.ForEach(l => Links.Add(new ProjectLinkView(l.Name, l.Uri)));
        }

        public int ProjectId { get; }
        public string Name { get; }
        public List<int> ProjectTypes { get; }
        public string Info { get; }
        public ProjectStatus ProjectStatus { get; }
        public ImageView LandingImage { get; }
        public List<ProjectMembershipView> ProjectMemberships { get; }
        public List<ImageView> Screenshots { get; }
        public List<ProjectLinkView> Links { get; set; }
    }
}
