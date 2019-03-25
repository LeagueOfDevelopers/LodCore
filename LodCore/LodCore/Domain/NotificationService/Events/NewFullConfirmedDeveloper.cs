using Journalist;

namespace LodCore.Domain.NotificationService
{
    public class NewFullConfirmedDeveloper : EventInfoBase
    {
        public NewFullConfirmedDeveloper(int newDeveloperId, string firstName, string lastName)
        {
            Require.Positive(newDeveloperId, nameof(newDeveloperId));
            Require.NotEmpty(firstName, nameof(firstName));
            Require.NotEmpty(lastName, nameof(lastName));

            NewDeveloperId = newDeveloperId;
            FirstName = firstName;
            LastName = lastName;
        }

        public int NewDeveloperId { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}