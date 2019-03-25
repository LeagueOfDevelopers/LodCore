namespace LodCore.QueryService.DTOs
{
    public class ProjectMembershipDto
    {
        public int MembershipId { get; set; }
        public int DeveloperId { get; set; }
        public string Role { get; set; }
        public int ProjectId { get; set; }
    }
}