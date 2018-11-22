using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using Journalist.Extensions;
using LodCore.Domain.UserManagement;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserRepository, IUsersRepository
    {
        public UserRepository()
        {
        }

        public int CreateAccount(Account account)
        {
            Require.NotNull(account, nameof(account));
            /*
            var session = _sessionProvider.GetCurrentSession();
            var savedAccountId = (int) session.Save(account);
            // todo: perform check for NRE
            return savedAccountId;*/
            return 0;
        }

        public void UpdateAccount(Account account)
        {
            /*
            Require.NotNull(account, nameof(account));

            var session = _sessionProvider.GetCurrentSession();
            session.Update(account);*/
        }

        public Account GetAccount(int accountId)
        {
            /*
            Require.Positive(accountId, nameof(accountId));

            var session = _sessionProvider.GetCurrentSession();
            var account = session.Get<Account>(accountId);
            return account;*/
            return null;
        }

        public Account GetAccountByLinkToGithubProfile(string link)
        {
            /*
            Require.NotEmpty(link, nameof(link));

            var session = _sessionProvider.GetCurrentSession();
            var account = session.Query<Account>().Where(user => user
                                 .Profile.LinkToGithubProfile == new Uri(link)).SingleOrDefault();
            return account;*/
            return null;
        }

        public List<Account> GetAllAccounts(Func<Account, bool> predicate = null)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            return predicate == null
                ? session.Query<Account>().ToList()
                : session.Query<Account>().Where(predicate).ToList();*/
            return null;
        }

        public List<Account> GetSomeAccounts<TComparable>(
            int skipCount, 
            int takeCount,
            Func<Account, TComparable> orderer, 
            Func<Account, bool> predicate = null)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            var query = session.Query<Account>();
            var customizedQuery = predicate == null
                ? query
                : query.Where(predicate);
            return customizedQuery.OrderBy(orderer).Skip(skipCount).Take(takeCount).ToList();
            */
            return null;
        }

        public int[] GetAllUsersIds()
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            return session.Query<Account>().SelectToArray(account => account.UserId);*/
            return null;
        }

        public int[] GetAllAdminIds()
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            return session.Query<Account>()
                .Where(account => account.Role == AccountRole.Administrator)
                .SelectToArray(account => account.UserId);*/
            return null;
        }
    }
}