using Journalist;

namespace LodCore.Models
{
    public class ProjectMembership
    {
        public ProjectMembership(int developerId, string firstName, string lastName, string role)
        {
            Require.Positive(developerId, nameof(developerId));
            Require.NotEmpty(firstName, nameof(firstName));
            Require.NotEmpty(lastName, nameof(lastName));
            Require.NotNull(role, nameof(role));

            DeveloperId = developerId;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }

        public int DeveloperId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Role { get; private set; }
    }
}