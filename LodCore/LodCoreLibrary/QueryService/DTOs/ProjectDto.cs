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

        public ProjectDto(int projectId, string name, string info, 
            ProjectStatus projectStatus, string bigPhotoUri, 
            string smallPhotoUri, string linkName, Uri uriLink)
        {
            ProjectId = projectId;
            Name = name;
            Info = info;
            ProjectStatus = projectStatus;
            BigPhotoUri = bigPhotoUri;
            SmallPhotoUri = smallPhotoUri;
            LinkName = linkName;
            UriLink = uriLink;
        }

        public int ProjectId { get; }
        public string Name { get; }
        public string Info { get; }
        public ProjectStatus ProjectStatus { get; }
        public string BigPhotoUri { get; }
        public string SmallPhotoUri { get; }
        public string LinkName { get; }
        public Uri UriLink { get; }
    }
}
