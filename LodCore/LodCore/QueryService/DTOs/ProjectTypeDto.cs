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

        public ProjectTypeDto(ProjectType id, int projectKey)
        {
            ProjectKey = projectKey;
            Id = id;
        }

        public int ProjectKey { get; set; }
        public ProjectType Id { get; set; }
    }
}
