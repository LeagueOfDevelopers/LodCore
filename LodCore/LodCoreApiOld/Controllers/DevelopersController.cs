using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using Journalist;
using LodCoreApiOld.App_Data;
using LodCoreApiOld.App_Data.Mappers;
using LodCoreApiOld.Authorization;
using LodCoreApiOld.Models;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.Exceptions;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Facades;
using Serilog;
using NotificationSetting = LodCoreApiOld.Models.NotificationSetting;
using PasswordChangeRequest = LodCoreApiOld.Models.PasswordChangeRequest;

namespace LodCoreApiOld.Controllers
{
    public class DevelopersController : ApiController
    {
        private const string PageParamName = "page";
        private readonly IConfirmationService _confirmationService;
        private readonly DevelopersMapper _mapper;

        private readonly IPaginationWrapper<Account> _paginationWrapper;
        private readonly IPasswordManager _passwordManager;

        private readonly IUserManager _userManager;
        private readonly IUserPresentationProvider _userPresentationProvider;

        public DevelopersController(
            IUserManager userManager,
            DevelopersMapper mapper,
            IConfirmationService confirmationService,
            IUserPresentationProvider userPresentationProvider,
            IPaginationWrapper<Account> paginationWrapper,
            IPasswordManager passwordManager)
        {
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(mapper, nameof(mapper));
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(userPresentationProvider, nameof(userPresentationProvider));

            _userManager = userManager;
            _mapper = mapper;
            _confirmationService = confirmationService;
            _userPresentationProvider = userPresentationProvider;
            _paginationWrapper = paginationWrapper;
            _passwordManager = passwordManager;
        }

        [HttpGet]
        [Route("developers/random/{count}")]
        public IEnumerable<IndexPageDeveloper> GetRandomIndexPageDevelopers(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var users = _userManager.GetUserList(
                    GetAccountFilter())
                .GetRandom(count);
            var indexPageDevelopers = users.Select(_mapper.ToIndexPageDeveloper);
            return indexPageDevelopers;
        }

        [HttpGet]
        [Route("developers/all")]
        public PaginableObject GetAllDevelopers()
        {
            var users = _userManager.GetUserList(GetAccountFilter());
            var developerPageDevelopers = users.Select(_mapper.ToDeveloperPageDeveloper);
            return _paginationWrapper.WrapResponse(developerPageDevelopers);
        }

        [HttpGet]
        [Route("developers")]
        public PaginableObject GetDevelopersByPage()
        {
            var paramsQuery = Request.RequestUri.Query;

            var paramsNameValueConnection = HttpUtility.ParseQueryString(paramsQuery);

            int pageId;

            if (!paramsNameValueConnection.AllKeys.Contains(PageParamName))
            {
                pageId = 0;
            }
            else
            {
                var pageParamValue = paramsNameValueConnection[PageParamName];
                if (!int.TryParse(pageParamValue, out pageId))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var users = _userManager.GetUserList(pageId,
                GetAccountFilter());
            var developerPageDevelopers = users.Select(_mapper.ToDeveloperPageDeveloper);

            return _paginationWrapper.WrapResponse(developerPageDevelopers, GetAccountFilterExpression());
        }

        [HttpGet]
        [Route("developers/search/{searchString}")]
        public IEnumerable<DeveloperPageDeveloper> SearchDevelopers(string searchString)
        {
            var users = _userManager.GetUserList(searchString);

            var developerPageDevelopers = users
                .Where(GetAccountFilter())
                .Select(_mapper.ToDeveloperPageDeveloper);
            return developerPageDevelopers;
        }

        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [Route("developers/{id}")]
        public IHttpActionResult GetDeveloper(int id)
        {
            Require.Positive(id, nameof(id));
            try
            {
                var user = _userManager.GetUser(id);
                if (!GetAccountFilter()(user)) throw new AccountNotFoundException();

                if (HttpContext.Current.User.IsInRole(AccountRole.User)) return Ok(_mapper.ToDeveloper(user));

                return Ok(_mapper.ToGuestDeveloper(user));
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", id.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("developers")]
        public IHttpActionResult RegisterNewDeveloper([FromBody] RegisterDeveloperRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createAccountRequest = new CreateAccountRequest(
                new MailAddress(request.Email),
                request.LastName,
                request.FirstName,
                request.Password,
                new Profile
                {
                    InstituteName = request.InstituteName,
                    PhoneNumber = request.PhoneNumber,
                    Specialization = request.StudyingProfile,
                    StudentAccessionYear = request.AccessionYear,
                    IsGraduated = request.IsGraduated,
                    StudyingDirection = request.Department,
                    VkProfileUri = request.VkProfileUri == null ? null : new Uri(request.VkProfileUri)
                });
            try
            {
                var newUserId = _userManager.CreateUser(createAccountRequest);
                return Ok(newUserId);
            }
            catch (AccountAlreadyExistsException ex)
            {
                Log.Error("Failed to register user with email={0}. {1} StackTrace: {2}", request.Email, ex.Message,
                    ex.StackTrace);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Conflict));
            }
        }

        [HttpPut]
        [Route("developers/{id}")]
        [Authorization(AccountRole.User)]
        public IHttpActionResult UpdateProfile(int id, [FromBody] ProfileUpdateRequest profileRequest)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(profileRequest, nameof(profileRequest));

            User.AssertResourceOwnerOrAdmin(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Account userToChange;
            try
            {
                userToChange = _userManager.GetUser(id);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", id.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }

            var profile = new Profile
            {
                Image = profileRequest.Image,
                InstituteName = profileRequest.InstituteName,
                PhoneNumber = profileRequest.PhoneNumber,
                Specialization = profileRequest.Specialization,
                StudentAccessionYear = profileRequest.StudentAccessionYear,
                IsGraduated = profileRequest.IsGraduated,
                StudyingDirection = profileRequest.StudyingDirection,
                VkProfileUri = string.IsNullOrWhiteSpace(profileRequest.VkProfileUri)
                    ? null
                    : new Uri(profileRequest.VkProfileUri),
                LinkToGithubProfile = profileRequest.LinkToGithubProfile
            };

            userToChange.Profile = profile;

            _userManager.UpdateUser(userToChange);

            return Ok();
        }

        [HttpPut]
        [Route("password")]
        public IHttpActionResult ChangePassword([FromBody] PasswordChangeRequest request)
        {
            Account userToChange;
            if (User.IsInRole(AccountRole.User))
                userToChange = _userManager.GetUser(User.Identity.GetId());
            else if (request.Token != null)
                userToChange = _passwordManager.GetUserByPasswordRecoveryToken(request.Token);
            else
                return BadRequest();

            if (userToChange == null) return NotFound();

            var userId = userToChange.UserId;

            if (!Password.IsStringCorrectPassword(request.NewPassword)) return BadRequest();
            _userManager.ChangeUserPassword(userId, request.NewPassword);
            return Ok();
        }

        [HttpPut]
        [Route("developers/notificationsettings/{id}")]
        [Authorization(AccountRole.User)]
        public IHttpActionResult UpdateNotificationSetiings(int id,
            [FromBody] NotificationSetting[] notificationSettings)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(notificationSettings, nameof(notificationSettings));

            User.AssertResourceOwnerOrAdmin(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!User.IsInRole(AccountRole.Administrator))
                if (notificationSettings?.Any(
                        setting => setting.NotificationSettingValue == NotificationSettingValue.DontSend) ?? false)
                    return BadRequest("You can't turn off notification sending");

            try
            {
                _userManager.GetUser(id);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", id.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }

            foreach (var notificationSetting in notificationSettings)
                _userPresentationProvider.UpdateNotificationSetting(
                    new LodCoreLibraryOld.Domain.UserManagement.NotificationSetting(
                        id,
                        notificationSetting.NotificationType,
                        notificationSetting.NotificationSettingValue));
            return Ok();
        }

        [HttpGet]
        [Route("developers/notificationsettings/{id}")]
        [Authorization(AccountRole.User)]
        public NotificationSetting[] GetNotificationSettings(int id)
        {
            Require.Positive(id, nameof(id));
            try
            {
                var user = _userManager.GetUser(id);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", id.ToString(), ex.Message,
                    ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return
                Enum.GetValues(typeof(NotificationType))
                    .Cast<NotificationType>()
                    .Select(name => new NotificationSetting
                    {
                        NotificationType = name,
                        NotificationSettingValue = _userPresentationProvider.GetUserEventSettings(id, name.ToString())
                    }).ToArray();
        }

        [HttpPost]
        [Route("developers/confirmation/{confirmationToken}")]
        public IHttpActionResult ConfirmEmail(string confirmationToken)
        {
            try
            {
                _confirmationService.ConfirmEmail(confirmationToken);
            }
            catch (TokenNotFoundException ex)
            {
                Log.Error("Failed to find confirmationToken={0}. {1} StackTrace: {2}", confirmationToken, ex.Message,
                    ex.StackTrace);
                return BadRequest("Token not found");
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Failed to confirm email by confirmationToken={0}. {1} StackTrace: {2}", confirmationToken,
                    ex.Message, ex.StackTrace);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("password/recovery")]
        public IHttpActionResult InitiateProcedureOfPasswordRecovery([FromBody] Credentials credentials)
        {
            var accountToRecover = _userManager.GetUserList(user =>
                user.Email.Address == credentials.Email).SingleOrDefault();

            if (accountToRecover != null)
            {
                _userManager.InitiatePasswordChangingProcedure(accountToRecover.UserId);
                return Ok();
            }

            return NotFound();
        }

        private Func<Account, bool> GetAccountFilter()
        {
            if (User.IsInRole(AccountRole.Administrator))
                return account => account.ConfirmationStatus == ConfirmationStatus.EmailConfirmed
                                  || account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed;

            return account => account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed && !account.IsHidden;
        }

        private Expression<Func<Account, bool>> GetAccountFilterExpression()
        {
            if (User.IsInRole(AccountRole.Administrator))
                return account => account.ConfirmationStatus == ConfirmationStatus.EmailConfirmed
                                  || account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed;

            return account => account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed && !account.IsHidden;
        }
    }
}