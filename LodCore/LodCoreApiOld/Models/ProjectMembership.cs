using Journalist;

namespace LodCoreApiOld.Models
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

        public int DeveloperId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Role { get; }
    }
}