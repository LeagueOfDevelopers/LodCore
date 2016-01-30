using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using Journalist.Extensions;
using NHibernate.Linq;
using NotificationService;
using UserManagement.Domain;
using UserManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository, IUsersRepository, ProjectManagement.Infrastructure.IUserRepository
    {
        private readonly DatabaseSessionProvider _sessionProvider;

        public UserRepository(DatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));

            _sessionProvider = sessionProvider;
        }

        public int CreateAccount(Account account)
        {
            Require.NotNull(account, nameof(account));

            var session = _sessionProvider.GetCurrentSession();
            var savedAccountId = (int) session.Save(account);
            // todo: perform check for NRE
            return savedAccountId;
        }

        public void UpdateAccount(Account account)
        {
            Require.NotNull(account, nameof(account));

            var session = _sessionProvider.GetCurrentSession();
            session.Update(account);
        }

        public Account GetAccount(int accountId)
        {
            Require.Positive(accountId, nameof(accountId));

            var session = _sessionProvider.GetCurrentSession();
            var account = session.Get<Account>(accountId);
            return account;
        }

        public List<Account> GetAllAccounts(Func<Account, bool> predicate = null)
        {
            var session = _sessionProvider.GetCurrentSession();
            return predicate == null
                ? session.Query<Account>().ToList()
                : session.Query<Account>().Where(predicate).ToList();
        }

        public int GetUserRedmineId(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var account = GetAccount(userId);
            return account.RedmineUserId;
        }

        public int GetUserGitlabId(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var account = GetAccount(userId);
            return account.GitlabUserId;
        }

        public int[] GetAllUsersIds()
        {
            var session = _sessionProvider.GetCurrentSession();
            return session.Query<Account>().SelectToArray(account => account.UserId);
        }

        public int[] GetAllAdminIds()
        {
            var session = _sessionProvider.GetCurrentSession();
            return session.Query<Account>()
                .Where(account => account.Role == AccountRole.Administrator)
                .SelectToArray(account => account.UserId);
        }
    }
}