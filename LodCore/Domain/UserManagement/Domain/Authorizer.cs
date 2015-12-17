using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Journalist;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class Authorizer : IAuthorizer
    {
        public Authorizer(TimeSpan tokenLifeTime, IUserRepository userRepository)
        {
            Require.NotNull(userRepository, nameof(userRepository));

            TokenLifeTime = tokenLifeTime;
            _userRepository = userRepository;
        }

        public bool CheckAuthorized(string authorizationToken, int userId)
        {
            Require.NotEmpty(authorizationToken, nameof(authorizationToken));
            Require.Positive(userId, nameof(userId));

            if (!_tokensWithGenerationTime.ContainsKey(authorizationToken))
            {
                return false;
            }
            var token = _tokensWithGenerationTime[authorizationToken];
            if (token.UserId != userId)
            {
                return false;
            }
            if (token.CreationTime + TokenLifeTime < DateTime.Now)
            {
                return false;
            }

            token.CreationTime = DateTime.Now;
            return true;
        }

        public AuthorizationToken Authorize(string email, Password password)
        {
            Require.NotEmpty(email, nameof(email));
            Require.NotNull(password, nameof(password));

            var userAccount = _userRepository
                .GetAllAccounts(account => account.Email.Address == email)
                .SingleOrDefault();
            if (userAccount == null)
            {
                throw new AccountNotFoundException("There is no account with such email");
            }

            if (userAccount.Password.Pass != password.Pass)
            {
                throw new UnauthorizedAccessException("Wrong password");
            }

            var token = GenerateNewToken(userAccount.UserId);
            _tokensWithGenerationTime.Add(token.Token, token);
            return token;
        }

        public TimeSpan TokenLifeTime { get; }

        private static AuthorizationToken GenerateNewToken(int userId)
        {
            var guid = Guid.NewGuid();
            var token = BitConverter.ToString(guid.ToByteArray());
            token = token.Replace("-", "");
            return new AuthorizationToken(userId, token, DateTime.Now);
        }

        private readonly Dictionary<string, AuthorizationToken> _tokensWithGenerationTime 
            = new Dictionary<string, AuthorizationToken>();
        private readonly IUserRepository _userRepository;
    }
}