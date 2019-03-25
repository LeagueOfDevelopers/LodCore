using System;
using Journalist;

namespace LodCore.Domain.UserManagement
{
    public class AuthorizationTokenInfo
    {
        public AuthorizationTokenInfo(int userId, string token, DateTime creationTime, AccountRole role)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(token, nameof(token));

            UserId = userId;
            Token = token;
            CreationTime = creationTime;
            Role = role;
        }

        public int UserId { get; }

        public AccountRole Role { get; }

        public string Token { get; }

        public DateTime CreationTime { get; set; }
    }
}