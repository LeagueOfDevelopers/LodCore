using FrontendServices.Models;
using ProjectManagement.Domain;

namespace FrontendServices.App_Data.Mappers
{
    public class ProjectsMapper
    {
        public IndexPageProject FromDomainEntity(Project project)
        {
            return new IndexPageProject(project.ProjectId, project.LandingImageUri, project.Name);
        } 
    }
}