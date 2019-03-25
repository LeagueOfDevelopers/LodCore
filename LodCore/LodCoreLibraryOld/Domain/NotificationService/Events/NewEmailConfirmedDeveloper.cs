using Journalist;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public class NewEmailConfirmedDeveloper : EventInfoBase
    {
        public NewEmailConfirmedDeveloper(int userId, string firstName, string lastName)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(firstName, nameof(firstName));
            Require.NotEmpty(lastName, nameof(lastName));

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public int UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}