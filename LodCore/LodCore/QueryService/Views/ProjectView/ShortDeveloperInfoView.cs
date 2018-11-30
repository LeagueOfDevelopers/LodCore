using LodCore.Domain.ProjectManagment;
using LodCore.Infrastructure.DataAccess.Pagination;
using LodCore.QueryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCore.QueryService.Views.ProjectView
{
    public class ShortDeveloperInfoView
    {
        public ShortDeveloperInfoView(dynamic project)
        {
            Id = project.ProjectId;
            Name = project.Name;
            ProjectStatus = (ProjectStatus)project.ProjectStatus;
            LandingImage = new ImageView(project.BigPhotoUri, project.SmallPhotoUri);
        }

        public int Id { get; }
        public string Name { get; }
        public ProjectStatus ProjectStatus { get; }
        public ImageView LandingImage { get; }
    }
}
