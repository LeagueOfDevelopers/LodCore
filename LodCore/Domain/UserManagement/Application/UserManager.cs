using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Journalist;
using NHibernate.Linq;
using UserManagement.Domain;

namespace UserManagement.Application
{
    public class UserManager : IUserManager
    {
        public UserManager(DatabaseSessionProvider sessionProvider, IConfirmationService confirmationService)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _sessionProvider = sessionProvider;
            _confirmationService = confirmationService;
        }
        
        public List<Account> GetUserList(Func<Account, bool> criteria = null)
        {
            using (var session = _sessionProvider.OpenSession())
            {
                var accounts = session.Query<Account>();
                var accountsList = criteria == null 
                    ? accounts.ToList() 
                    : accounts.Where(criteria).ToList();
                return accountsList;
            }
        }

        public Account GetUser(int userId)
        {
            using (var session = _sessionProvider.OpenSession())
            {
                return session.Get<Account>(userId);
            }
        }

        public void CreateUser(CreateAccountRequest request)
        {
            if (IsExist(request.Email))
            {
                //todo: return an error
                return;
            }

            var account = new Account(
                request.Firstname, 
                request.Lastname, 
                request.Email, 
                GetPasswordHash(request.Password),
                ConfirmationStatus.Unconfirmed, 
                null);

            int id;
            using (var session = _sessionProvider.OpenSession())
            {
                id = ((Account) session.Save(account)).UserId;
            }

            _confirmationService.SetupEmailConfirmation(id);
        }

        public Task UpdateUser(Account account)
        {
            throw new NotImplementedException();
        }

        private bool IsExist(string email)
        {
            using (var session = _sessionProvider.OpenSession())
            {
                return session.Query<Account>().Any(account => account.Email == email);
            }    
        }

        //todo: refactor in another helper class
        private static string GetPasswordHash(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5Data = md5.ComputeHash(data);
            return Convert.ToBase64String(md5Data);
        }

        private readonly DatabaseSessionProvider _sessionProvider;
        private readonly IConfirmationService _confirmationService;
    }
}