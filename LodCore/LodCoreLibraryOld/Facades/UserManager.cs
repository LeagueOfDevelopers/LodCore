using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Journalist;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.Exceptions;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.DataAccess.Repositories;
using LodCoreLibraryOld.Infrastructure.EventBus;
using NHibernate.Util;

namespace LodCoreLibraryOld.Facades
{
    public class UserManager : IUserManager
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ProjectPaginationSettings _paginationSettings;
        private readonly IPasswordManager _passwordManager;
        private readonly IProjectMembershipRepostiory _projectMembershipRepostiory;
        private readonly IUserRepository _userRepository;

        public UserManager(
            IUserRepository userRepository,
            IConfirmationService confirmationService,
            ProjectPaginationSettings paginationSettings,
            IProjectMembershipRepostiory projectMembershipRepostiory,
            ApplicationLocationSettings applicationLocationSettings,
            IPasswordManager passwordManager,
            IEventPublisher eventPublisher)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(paginationSettings, nameof(paginationSettings));
            Require.NotNull(projectMembershipRepostiory, nameof(projectMembershipRepostiory));
            Require.NotNull(applicationLocationSettings, nameof(applicationLocationSettings));
            Require.NotNull(passwordManager, nameof(passwordManager));
            Require.NotNull(eventPublisher, nameof(eventPublisher));

            _userRepository = userRepository;
            _confirmationService = confirmationService;
            _paginationSettings = paginationSettings;
            _projectMembershipRepostiory = projectMembershipRepostiory;
            _passwordManager = passwordManager;
            _eventPublisher = eventPublisher;
        }

        public List<Account> GetUserList(Func<Account, bool> criteria = null)
        {
            return _userRepository.GetAllAccounts(criteria);
        }

        public List<Account> GetUserList(int pageNumber, Func<Account, bool> criteria = null)
        {
            var projectToSkip = _paginationSettings.NumberOfProjects * pageNumber;
            var projectsToTake = _paginationSettings.NumberOfProjects;

            return _userRepository.GetSomeAccounts(
                projectToSkip,
                projectsToTake,
                account => account.RegistrationTime,
                criteria);
        }

        public Account GetUser(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var account = _userRepository.GetAccount(userId);
            if (account == null) throw new AccountNotFoundException();

            return account;
        }

        public Account GetUserByLinkToGithubProfile(string link)
        {
            Require.NotEmpty(link, nameof(link));

            var account = _userRepository.GetAccountByLinkToGithubProfile(link);

            return account;
        }

        public int CreateUser(CreateAccountRequest request)
        {
            Require.NotNull(request, nameof(request));

            var doesExist = EnumerableExtensions.Any(GetUserList(account => account.Email.Address == request.Email));
            if (doesExist) throw new AccountAlreadyExistsException();

            var newAccount = new Account(
                request.Firstname,
                request.Lastname,
                new MailAddress(request.Email),
                new Password(request.Password),
                AccountRole.User,
                ConfirmationStatus.Unconfirmed,
                DateTime.Now,
                request.Profile);

            var userId = _userRepository.CreateAccount(newAccount);

            return userId;
        }

        public int CreateUserTemplate(CreateAccountRequest accountRequest)
        {
            var templateAccount = new Account(
                accountRequest.Firstname,
                accountRequest.Lastname,
                AccountRole.User,
                ConfirmationStatus.EmailConfirmed,
                DateTime.Now,
                accountRequest.Profile);

            var userId = _userRepository.CreateAccount(templateAccount);

            return userId;
        }

        public void UpdateUser(Account account)
        {
            Require.NotNull(account, nameof(account));

            var accountExists = _userRepository.GetAccount(account.UserId) != null;
            if (!accountExists) throw new AccountNotFoundException();

            _userRepository.UpdateAccount(account);
        }

        public void ChangeUserPassword(int userId, string newPassword)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(newPassword, nameof(newPassword));

            var user = _userRepository.GetAccount(userId);

            var accountExists = user != null;
            if (!accountExists) throw new AccountNotFoundException();

            _passwordManager.UpdateUserPassword(userId, newPassword);

            var requestToDelete = _passwordManager.GetPasswordChangeRequest(userId);

            if (requestToDelete != null) _passwordManager.DeletePasswordChangeRequest(requestToDelete);
        }

        public void InitiatePasswordChangingProcedure(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToInitiateProcedure = GetUser(userId);

            var @event = _passwordManager.GetPasswordChangeRequest(userId);

            if (@event == null)
            {
                @event = new PasswordChangeRequest(userId, TokenGenerator.GenerateToken());
                _passwordManager.SavePasswordChangeRequest(@event);
            }

            _eventPublisher.PublishEvent(@event);
        }

        public List<Account> GetUserList(string searchString)
        {
            Require.NotEmpty(searchString, nameof(searchString));

            var allProjectMemberships = _projectMembershipRepostiory.GetAllProjectMemberships().ToList();

            var allUsers = new HashSet<Account>(_userRepository.GetAllAccounts());

            var allUsersToSearchByRole =
                new HashSet<Account>(
                    allProjectMemberships.Select(membership => allUsers.Single(account =>
                        account.UserId == membership.DeveloperId)));

            var userRolesDictionary = allUsersToSearchByRole.ToDictionary(user => user,
                user =>
                    allProjectMemberships.Where(membership => membership.DeveloperId == user.UserId)
                        .Select(that => that.Role));

            return userRolesDictionary.Where(
                    pair =>
                        pair.Value.Any(role => Extensions.Contains(role, searchString)))
                .Select(pair => pair.Key)
                .Union(
                    allUsers.Where(
                        account =>
                            Extensions.Contains($"{account.Firstname} {account.Lastname}",
                                searchString))).ToList();
        }

        //TODO: доработать
        public Account GetUserByCredentials(string email, string password)
        {
            var account = _userRepository.GetAllAccounts().First(a => a.Email == new MailAddress(email));

            if (account.Password.Value == password)
                return account;
            throw new Exception("Неверный пароль");
        }
    }
}