using System;
using Journalist;

namespace UserManagement.Domain
{
    public class AuthorizationToken
    {
        public AuthorizationToken(int userId, string token, DateTime creationTime)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(token, nameof(token));

            UserId = userId;
            Token = token;
            CreationTime = creationTime;
        }

        public int UserId { get; private set; } 

        public string Token { get; private set; }

        public DateTime CreationTime { get; set; }
    }
}