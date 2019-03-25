using LodCore.Domain.ProjectManagment;

namespace LodCore.QueryService.DTOs
{
    public class ProjectTypeDto
    {
        public ProjectTypeDto()
        {
        }

        public ProjectTypeDto(ProjectType projectType, int projectId)
        {
            Type = projectType;
            ProjectId = projectId;
        }

        public int ProjectId { get; set; }
        public ProjectType Type { get; set; }
    }
}