using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Dapper;
using Journalist;
using Journalist.Extensions;
using LodCore.Domain.UserManagement;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using LodCore.Common;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserRepository, IUsersRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
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
            Require.Positive(accountId, nameof(accountId));

            var sql = "SELECT * " +
                "FROM accounts " +
                "WHERE UserId =@UserId ";
            Account account;
            using (var connection = new MySqlConnection(_connectionString))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                account = connection.Query<dynamic>(sql, new { UserId = accountId })
                    .Select(p =>
                    new Account(p.Firstname,
                                p.Lastname,
                                new MailAddress(p.Email),
                                Password.FromPlainString(p.Password),
                                (AccountRole)p.AccountRole,
                                (ConfirmationStatus)p.ConfirmationStatus,
                                p.RegistrationTime,
                                new Profile
                                {
                                    Image = new Image(new Uri(p.BigPhotoUri), new Uri(p.SmallPhotoUri)),
                                    VkProfileUri = new Uri(p.VkProfileUri),
                                    LinkToGithubProfile = new Uri(p.GitHubProfileUri),
                                    PhoneNumber = p.PhoneNumber,
                                    StudentAccessionYear = p.StudentAccessionYear,
                                    IsGraduated = p.IsGraduated,
                                    StudyingDirection = p.StudyingDirection,
                                    InstituteName = p.InstituteName,
                                    Specialization = p.Specialization
                                }))
                     .FirstOrDefault();
            }

            return account;
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
            var sql = "SELECT * FROM accounts ";

            List<Account> accounts;
            using (var connection = new MySqlConnection(_connectionString))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                accounts = connection.Query<dynamic>(sql)
                    .Select(p =>
                        new Account(p.Firstname,
                                p.Lastname,
                                new MailAddress(p.Email),
                                Password.FromPlainString(p.Password),
                                (AccountRole)p.AccountRole,
                                (ConfirmationStatus)p.ConfirmationStatus,
                                p.RegistrationTime,
                                new Profile
                                {
                                    Image = new Image(new Uri(p.BigPhotoUri), new Uri(p.SmallPhotoUri)),
                                    VkProfileUri = new Uri(p.VkProfileUri),
                                    LinkToGithubProfile = new Uri(p.GitHubProfileUri),
                                    PhoneNumber = p.PhoneNumber,
                                    StudentAccessionYear = p.StudentAccessionYear,
                                    IsGraduated = p.IsGraduated,
                                    StudyingDirection = p.StudyingDirection,
                                    InstituteName = p.InstituteName,
                                    Specialization = p.Specialization
                                }))
                     .ToList();
            }
            return accounts;
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