using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using LodCoreLibrary.Infrastructure.DataAccess.Repositories;
using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.Exceptions;

namespace LodCoreLibrary.Domain.UserManagement
{
    public class Authorizer : IAuthorizer
    {
        public Authorizer(TimeSpan tokenLifeTime, IUserRepository userRepository)
        {
            Require.NotNull(userRepository, nameof(userRepository));

            TokenLifeTime = tokenLifeTime;
            _userRepository = userRepository;
        }

        public AuthorizationTokenInfo GetTokenInfo(string authorizationToken)
        {
            Require.NotEmpty(authorizationToken, nameof(authorizationToken));

            if (!_tokensWithGenerationTime.ContainsKey(authorizationToken))
            {
                return null;
            }
            var token = _tokensWithGenerationTime[authorizationToken];

            if (token.CreationTime + TokenLifeTime < DateTime.Now)
            {
                _tokensWithGenerationTime.TryRemove(token.Token, out token);
                return null;
            }

            token.CreationTime = DateTime.Now;
            return token;
        }

        public AuthorizationTokenInfo Authorize(string email, Password password)
        {
            Require.NotEmpty(email, nameof(email));
            Require.NotNull(password, nameof(password));

            var userAccount = _userRepository
                .GetAllAccounts(
                    account => account.Email?.Address == email
                               && account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed)
                .SingleOrDefault();
            if (userAccount == null)
            {
                throw new AccountNotFoundException("There is no account with such email");
            }

            if (userAccount.Password.Value != password.Value)
            {
                throw new UnauthorizedAccessException("Wrong password");
            }
            var token = GetToken(userAccount);
            return token;
        }

        public AuthorizationTokenInfo AuthorizeWithGithub(string link)
        {
            Require.NotEmpty(link, nameof(link));

            var account = _userRepository.GetAllAccounts(a => a.Profile
                                         .LinkToGithubProfile == new Uri(link) &&
                                         a.ConfirmationStatus == ConfirmationStatus.FullyConfirmed)
                                         .SingleOrDefault();
            if (account == null)
            {
                throw new AccountNotFoundException("There is no account with such link to github profile");
            }
            var token = GetToken(account);
            return token;
        }

        public TimeSpan TokenLifeTime { get; }

        private AuthorizationTokenInfo GetToken(Account account)
        {
            var existantToken = TakeTokenByUserId(account.UserId);
            if (existantToken != null)
            {
                return existantToken;
            }
            var token = GenerateNewToken(account);
            _tokensWithGenerationTime.AddOrUpdate(token.Token, token, (oldToken, info) => token);
            return token;
        }

        private AuthorizationTokenInfo TakeTokenByUserId(int userId)
        {
            var pair = _tokensWithGenerationTime.SingleOrDefault(token => token.Value.UserId == userId);
            if (!pair.Equals(default(KeyValuePair<string, AuthorizationTokenInfo>)))
            {
                return pair.Value;
            }

            return null;
        }

        private static AuthorizationTokenInfo GenerateNewToken(Account account)
        {
            var guid = Guid.NewGuid();
            var token = BitConverter.ToString(guid.ToByteArray());
            token = token.Replace("-", "");
            return new AuthorizationTokenInfo(account.UserId, token, DateTime.Now, account.Role);
        }

        private readonly ConcurrentDictionary<string, AuthorizationTokenInfo> _tokensWithGenerationTime
            = new ConcurrentDictionary<string, AuthorizationTokenInfo>();

        private readonly IUserRepository _userRepository;
    }
}