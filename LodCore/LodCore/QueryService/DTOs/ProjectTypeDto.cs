using LodCore.Domain.ProjectManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
