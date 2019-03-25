namespace LodCoreLibraryOld.QueryService.Views.ProjectView
{
    public class ProjectMembershipView
    {
        public ProjectMembershipView(int developerId, string role)
        {
            DeveloperId = developerId;
            Role = role;
        }

        public int DeveloperId { get; }
        public string Role { get; }
    }
}