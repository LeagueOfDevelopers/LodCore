using System.Collections.Generic;
using Journalist;

namespace Mailing.AsyncMailing
{
    public class NotificationEmail
    {
        public NotificationEmail(int[] userIds, string notificationDescription)
        {
            Require.NotEmpty(userIds, nameof(userIds));
            Require.NotNull(notificationDescription, nameof(notificationDescription));
            
            UserIds = new HashSet<int>(userIds);
            NotificationDescription = notificationDescription;
        }

        protected NotificationEmail() { }

        public virtual int Id { get; protected set; }

        public virtual ISet<int> UserIds { get; protected set; }
        
        public virtual string NotificationDescription { get; protected set; } 
    }
}