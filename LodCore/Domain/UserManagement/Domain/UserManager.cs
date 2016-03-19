using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Common;
using Journalist;
using Journalist.Extensions;
using NHibernate.Util;
using NotificationService;
using ProjectManagement.Application;
using ProjectManagement.Infrastructure;
using UserManagement.Application;
using IUserRepository = UserManagement.Infrastructure.IUserRepository;

namespace UserManagement.Domain
{
    public class UserManager : IUserManager
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IUserRepository _userRepository;
        private readonly IProjectProvider _projectProvider;
        private readonly PaginationSettings _paginationSettings;
        private readonly IProjectMembershipRepostiory _projectMembershipRepostiory;

        public UserManager(
            IUserRepository userRepository,
            IConfirmationService confirmationService, PaginationSettings paginationSettings, IProjectProvider projectProvider, IProjectMembershipRepostiory projectMembershipRepostiory)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _userRepository = userRepository;
            _confirmationService = confirmationService;
            _paginationSettings = paginationSettings;
            _projectProvider = projectProvider;
            _projectMembershipRepostiory = projectMembershipRepostiory;
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

            var allProjectMemberships = _projectMembershipRepostiory.GetAllProjectMemberships().ToList();

            var allUsersToSearchByRole =
                new HashSet<Account>(
                    allProjectMemberships.Select(membership => _userRepository.GetAccount(membership.DeveloperId)));

            var userRolesDictionary = allUsersToSearchByRole.ToDictionary(user => user,
                user =>
                    allProjectMemberships.Where(membership => membership.DeveloperId == user.UserId)
                        .Select(that => that.Role));

            return _userRepository.SearchAccounts(searchString, userRolesDictionary);
        }
    }
}