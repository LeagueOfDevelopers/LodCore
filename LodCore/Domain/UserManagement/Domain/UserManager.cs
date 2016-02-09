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
        private readonly IGitlabUserRegistrar _gitlabUserRegistrar;
        private readonly IRedmineUserRegistrar _redmineUserRegistrar;
        private readonly IUserRepository _repository;

        public UserManager(
            IUserRepository repository,
            IConfirmationService confirmationService,
            IRedmineUserRegistrar redmineUserRegistrar,
            IGitlabUserRegistrar gitlabUserRegistrar)
        {
            Require.NotNull(repository, nameof(repository));
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(redmineUserRegistrar, nameof(redmineUserRegistrar));
            Require.NotNull(gitlabUserRegistrar, nameof(gitlabUserRegistrar));

            _repository = repository;
            _confirmationService = confirmationService;
            _redmineUserRegistrar = redmineUserRegistrar;
            _gitlabUserRegistrar = gitlabUserRegistrar;
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

            //todo: fix 39 task
            var redmineUserId = 1; /*_redmineUserRegistrar.RegisterUser(request);*/
            var gitlabUserId = _gitlabUserRegistrar.RegisterUser(request);

            var newAccount = new Account(
                request.Firstname,
                request.Lastname,
                new MailAddress(request.Email),
                new Password(request.Password),
                AccountRole.User,
                ConfirmationStatus.Unconfirmed,
                DateTime.Now,
                request.Profile,
                redmineUserId,
                gitlabUserId);

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