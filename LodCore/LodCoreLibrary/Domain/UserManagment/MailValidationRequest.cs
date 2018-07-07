using Journalist;
using LodCoreLibrary.Domain.NotificationService;

namespace LodCoreLibrary.Domain.UserManagement
{
    public class MailValidationRequest : EventInfoBase
    {
        public MailValidationRequest(int userId, string token)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(token, nameof(token));

            UserId = userId;
            Token = token;
        }

        protected MailValidationRequest()
        {
        }

        public virtual int UserId { get; protected set; }
        public virtual string Token { get; protected set; }
    }
}