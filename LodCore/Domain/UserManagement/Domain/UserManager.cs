using System;
using System.Collections.Generic;
using System.Net.Mail;
using Common;
using Journalist;
using NHibernate.Util;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class UserManager : IUserManager
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IUserRepository _repository;

        public UserManager(
            IUserRepository repository,
            IConfirmationService confirmationService)
        {
            Require.NotNull(repository, nameof(repository));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _repository = repository;
            _confirmationService = confirmationService;
        }

        public List<Account> GetUserList(Func<Account, bool> criteria = null)
        {
            return _repository.GetAllAccounts(criteria);
        }

        public Account GetUser(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var account = _repository.GetAccount(userId);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            return account;
        }

        public void CreateUser(CreateAccountRequest request)
        {
            Require.NotNull(request, nameof(request));

            var doesExist = GetUserList(account => account.Email.Address == request.Email).Any();
            if (doesExist)
            {
                throw new AccountAlreadyExistsException();
            }
            
           var newAccount = new Account(
                request.Firstname,
                request.Lastname,
                new MailAddress(request.Email),
                Password.FromPlainString(request.Password), 
                AccountRole.User,
                ConfirmationStatus.Unconfirmed,
                DateTime.Now,
                request.Profile,
                0,
                0);

            var userId = _repository.CreateAccount(newAccount);

            _confirmationService.SetupEmailConfirmation(userId);
        }

        public void UpdateUser(Account account)
        {
            Require.NotNull(account, nameof(account));

            var accountExists = _repository.GetAccount(account.UserId) != null;
            if (!accountExists)
            {
                throw new AccountNotFoundException();
            }

            _repository.UpdateAccount(account);
        }
    }
}