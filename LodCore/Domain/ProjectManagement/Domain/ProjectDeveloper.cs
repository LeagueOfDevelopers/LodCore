using Journalist;

namespace ProjectManagement.Domain
{
    public class ProjectDeveloper
    {
        public ProjectDeveloper(int developerId, string role)
        {
            Require.Positive(developerId, nameof(developerId));
            Require.NotEmpty(role, nameof(role));

            DeveloperId = developerId;
            Role = role;
        }

        public int DeveloperId { get; private set; }

        public string Role { get; private set; }
    }
}