using Common;
using Journalist;

namespace UserManagement.Domain.Events
{
    public class NewFullConfirmedDeveloper : EventInfoBase
    {
        public NewFullConfirmedDeveloper(int newDeveloperId)
        {
            Require.Positive(newDeveloperId, nameof(newDeveloperId));

            NewDeveloperId = newDeveloperId;
        }

        public int NewDeveloperId { get; private set; }
    }
}
