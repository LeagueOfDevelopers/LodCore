namespace LodCore.QueryService.DTOs
{
    public class DeveloperProjectDto
    {
        public DeveloperProjectDto(ProjectDto project, ProjectMembershipDto membership)
        {
            Project = project;
            Membership = membership;
        }

        public ProjectDto Project { get; set; }
        public ProjectMembershipDto Membership { get; set; }
    }
}