using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Common;
using Journalist;
using NHibernate.Util;
using NotificationService;
using ProjectManagement.Application;
using UserManagement.Application;
using UserManagement.Infrastructure;

namespace UserManagement.Domain
{
    public class UserManager : IUserManager
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IUserRepository _userRepository;
        private readonly IProjectProvider _projectProvider;
        private readonly PaginationSettings _paginationSettings;

        public UserManager(
            IUserRepository userRepository,
            IConfirmationService confirmationService, PaginationSettings paginationSettings, IProjectProvider projectProvider)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _userRepository = userRepository;
            _confirmationService = confirmationService;
            _paginationSettings = paginationSettings;
            _projectProvider = projectProvider;
        }

        public List<Account> GetUserList(Func<Account, bool> criteria = null)
        {
            return _userRepository.GetAllAccounts(criteria);
        }

        public List<Account> GetUserList(int pageNumber, Func<Account, bool> criteria = null)
        {
            var projectToSkip = _paginationSettings.PageSize*pageNumber;
            var projectsToTake = _paginationSettings.PageSize;

            return _userRepository.GetSomeAccounts(projectToSkip, projectsToTake, criteria);

        }

        public Account GetUser(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var account = _userRepository.GetAccount(userId);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            return account;
        }

        public void CreateUser(CreateAccountRequest request)
        {
            Require.NotNull(request, nameof(request));

            var doesExist = EnumerableExtensions.Any(GetUserList(account => account.Email.Address == request.Email));
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

            var userId = _userRepository.CreateAccount(newAccount);

            _confirmationService.SetupEmailConfirmation(userId);
        }

        public void UpdateUser(Account account)
        {
            Require.NotNull(account, nameof(account));

            var accountExists = _userRepository.GetAccount(account.UserId) != null;
            if (!accountExists)
            {
                throw new AccountNotFoundException();
            }

            _userRepository.UpdateAccount(account);
        }

        public List<Account> GetUserList(string searchString)
        {
            Require.NotEmpty(searchString, nameof(searchString));

            var userRolesDictionary =
                _userRepository.GetAllAccounts()
                    .ToDictionary(user => user,
                        user =>
                            _projectProvider.GetProjects(
                                project =>
                                    project.ProjectMemberships.Any(membership => membership.DeveloperId == user.UserId))
                                .Select(
                                    project =>
                                        project.ProjectMemberships.Single(
                                            membership => membership.DeveloperId == user.UserId).Role));


            return _userRepository.SearchAccounts(searchString, userRolesDictionary);
        }
    }
}