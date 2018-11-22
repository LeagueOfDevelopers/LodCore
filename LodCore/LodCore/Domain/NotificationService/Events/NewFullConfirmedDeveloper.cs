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

        public int NewDeveloperId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
    }
}
