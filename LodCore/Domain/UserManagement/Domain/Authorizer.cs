using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool CheckAuthorized(string authorizationToken)
        {
            if (!_tokensWithGenerationTime.ContainsKey(authorizationToken))
            {
                return false;
            }
            if (_tokensWithGenerationTime[authorizationToken] + TokenLifeTime < DateTime.Now)
            {
                return false;
            }

            _tokensWithGenerationTime[authorizationToken] = DateTime.Now;
            return true;
        }

        public string Authorize(string email, string password)
        {
            var userAccount = _userRepository.GetAllAccounts(account => account.Email == email).SingleOrDefault();
            if (userAccount == null)
            {
                throw new AccountNotFoundException("There is no account with such email");
            }

            if (userAccount.PasswordHash != password)
            {
                throw new UnauthorizedAccessException("Wrong password");
            }

            var token = GenerateNewToken();
            _tokensWithGenerationTime.Add(token, DateTime.Now);
            return token;
        }

        public TimeSpan TokenLifeTime { get; }

        private static string GenerateNewToken()
        {
            throw new NotImplementedException();
        }

        private readonly Dictionary<string, DateTime> _tokensWithGenerationTime = new Dictionary<string, DateTime>();
        private readonly IUserRepository _userRepository;
    }
}