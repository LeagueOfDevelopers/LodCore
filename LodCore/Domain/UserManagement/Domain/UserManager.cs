using System;
using System.Collections.Generic;
using System.Net.Mail;
using Common;
using Journalist;
using NHibernate.Util;
using NotificationService;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class UserManager : IUserManager
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IUserRepository _repository;
        private readonly PaginationSettings _paginationSettings;

        public UserManager(
            IUserRepository repository,
            IConfirmationService confirmationService, PaginationSettings paginationSettings)
        {
            Require.NotNull(repository, nameof(repository));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _repository = repository;
            _confirmationService = confirmationService;
            _paginationSettings = paginationSettings;
        }

        public List<Account> GetUserList(Func<Account, bool> criteria = null)
        {
            return _repository.GetAllAccounts(criteria);
        }

        public List<Account> GetUserList(int pageNumber, Func<Account, bool> criteria = null)
        {
            var projectToSkip = _paginationSettings.PageSize*pageNumber;
            var projectsToTake = _paginationSettings.PageSize;

            return _repository.GetSomeAccounts(projectToSkip, projectsToTake, criteria);

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

        public List<Account> GetUserList(string searchString)
        {
            Require.NotEmpty(searchString, nameof(searchString));

            return _repository.SearchAccounts(searchString);
        }
    }
}