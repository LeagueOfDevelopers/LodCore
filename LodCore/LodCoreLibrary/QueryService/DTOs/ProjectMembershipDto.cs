using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.DTOs
{
    public class ProjectMembershipDto
    {
        public ProjectMembershipDto()
        {
        }

        public int MembershipId { get; set; }
        public int DeveloperId { get; set; }
        public string Role { get; set; }
        public int ProjectId { get; set; }
    }
}
