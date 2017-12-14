using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Common;
using Journalist;
using Journalist.Extensions;
using NHibernate.Linq;
using NHibernate.Util;
using NotificationService;
using ProjectManagement.Infrastructure;
using UserManagement.Application;
using IMailer = UserManagement.Application.IMailer;
using IUserRepository = UserManagement.Infrastructure.IUserRepository;

namespace UserManagement.Domain
{
    public class UserManager : IUserManager
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IUserRepository _userRepository;
        private readonly PaginationSettings _paginationSettings;
        private readonly IProjectMembershipRepostiory _projectMembershipRepostiory;
        private readonly IMailer _mailer;
        private readonly IPasswordManager _passwordManager;
        private readonly ApplicationLocationSettings _applicationLocationSettings;


        public UserManager(
            IUserRepository userRepository,
            IConfirmationService confirmationService, PaginationSettings paginationSettings, IProjectMembershipRepostiory projectMembershipRepostiory, IMailer mailer, ApplicationLocationSettings applicationLocationSettings, IPasswordManager passwordManager)
        {
            Require.NotNull(userRepository, nameof(userRepository));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _userRepository = userRepository;
            _confirmationService = confirmationService;
            _paginationSettings = paginationSettings;
            _projectMembershipRepostiory = projectMembershipRepostiory;
            _mailer = mailer;
            _applicationLocationSettings = applicationLocationSettings;
            _passwordManager = passwordManager;
        }

        public List<Account> GetUserList(Func<Account, bool> criteria = null)
        {
            return _userRepository.GetAllAccounts(criteria);
        }

        public List<Account> GetUserList(int pageNumber, Func<Account, bool> criteria = null)
        {
            var projectToSkip = _paginationSettings.PageSize*pageNumber;
            var projectsToTake = _paginationSettings.PageSize;

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
                //Password.FromPlainString(request.Password),
                new Password(request.Password),
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

        public void ChangeUserPassword(int userId, string newPassword)
        {
            Require.Positive(userId, nameof(userId));
            Require.NotEmpty(newPassword, nameof(newPassword));

            var user = _userRepository.GetAccount(userId);

            var accountExists = user != null;
            if (!accountExists)
            {
                throw new AccountNotFoundException();
            }

            _passwordManager.UpdateUserPassword(userId, newPassword);

            user.Password = new Password(newPassword);

            _userRepository.UpdateAccount(user);

            var requestToDelete = _passwordManager.GetPasswordChangeRequest(userId);

            if (requestToDelete != null)
            {
                _passwordManager.DeletePasswordChangeRequest(requestToDelete);
            }
        }

        public void InitiatePasswordChangingProcedure(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToInitiateProcedure = GetUser(userId);

            var request = _passwordManager.GetPasswordChangeRequest(userId);

            if (_passwordManager.GetPasswordChangeRequest(userId) == null)
            {
                request = new PasswordChangeRequest(userId, TokenGenerator.GenerateToken());
            }

            var passwordChangeRequest = request;

            var link = $"{_applicationLocationSettings.FrontendAdress}/password/recovery/{passwordChangeRequest.Token}";

            _passwordManager.SavePasswordChangeRequest(request);

            _mailer.SendPasswordResetMail(link, userToInitiateProcedure.Email);
        }

        public List<Account> GetUserList(string searchString)
        {
            Require.NotEmpty(searchString, nameof(searchString));

            var allProjectMemberships = _projectMembershipRepostiory.GetAllProjectMemberships().ToList();

            var allUsers = new HashSet<Account>(_userRepository.GetAllAccounts());

            var allUsersToSearchByRole =
                new HashSet<Account>(
                    allProjectMemberships.Select(membership => allUsers.Single(account => account.UserId == membership.DeveloperId)));

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
    }
}