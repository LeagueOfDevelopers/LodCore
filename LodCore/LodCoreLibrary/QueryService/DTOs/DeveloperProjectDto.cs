using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.DTOs
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
