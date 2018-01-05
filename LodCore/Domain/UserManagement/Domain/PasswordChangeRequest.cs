using Common;
using Journalist;

namespace UserManagement.Domain
{
    public class PasswordChangeRequest : EventInfoBase
    {
        public PasswordChangeRequest(int userId, string token)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(token, nameof(token));

            UserId = userId;
            Token = token;
        }

        protected PasswordChangeRequest()
        {
            
        }

        public virtual int UserId { get; protected set; }
        public virtual string Token { get; protected set; }
    }
}