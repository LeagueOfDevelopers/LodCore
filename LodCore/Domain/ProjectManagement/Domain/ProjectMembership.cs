using Journalist;

namespace ProjectManagement.Domain
{
    public class ProjectMembership
    {
        public ProjectMembership(
            int developerId, 
            string role)
        {
            Require.Positive(developerId, nameof(developerId));
            Require.NotEmpty(role, nameof(role));

            DeveloperId = developerId;
            Role = role;
        }

        protected ProjectMembership()
        {
        }

        public virtual int MembershipId { get; protected set; }

        public virtual int DeveloperId { get; protected set; }

        public virtual string Role { get; protected set; }

    }
}