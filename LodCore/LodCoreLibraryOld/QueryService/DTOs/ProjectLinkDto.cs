using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibraryOld.QueryService.DTOs
{
    public class ProjectLinkDto
    {
        public ProjectLinkDto()
        {
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
    }
}
