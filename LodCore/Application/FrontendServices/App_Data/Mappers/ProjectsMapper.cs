using FrontendServices.Models;
using Journalist;
using ProjectManagement.Domain;

namespace FrontendServices.App_Data.Mappers
{
    public class ProjectsMapper
    {
        public IndexPageProject ToIndexPageProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            return new IndexPageProject(project.ProjectId, project.LandingImageUri, project.Name);
        }

        public ProjectPreview ToProjectPreview(Project project)
        {
            Require.NotNull(project, nameof(project));

            return new ProjectPreview(
                project.ProjectId, 
                project.LandingImageUri, 
                project.Name, 
                project.ProjectStatus, 
                project.ProjectType);
        }
    }
}