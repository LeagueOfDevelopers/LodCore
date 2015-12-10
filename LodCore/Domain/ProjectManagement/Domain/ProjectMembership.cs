using Journalist;

namespace ProjectManagement.Domain
{
    public class ProjectMembership
    {
        public ProjectMembership(
            int developerId, 
            string role,
            Project project)
        {
            Require.Positive(developerId, nameof(developerId));
            Require.NotEmpty(role, nameof(role));
            Require.NotNull(project, nameof(project));

            DeveloperId = developerId;
            Role = role;
            Project = project;
        }

        protected ProjectMembership()
        {
        }

        public virtual Project Project { get; protected set; }

        public virtual int MembershipId { get; protected set; }

        public virtual int DeveloperId { get; protected set; }

        public virtual string Role { get; protected set; }

    }
}