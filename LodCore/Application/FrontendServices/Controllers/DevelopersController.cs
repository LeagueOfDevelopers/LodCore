using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using Common;
using FrontendServices.App_Data;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Authorization;
using FrontendServices.Models;
using Journalist;
using UserManagement.Application;
using UserManagement.Domain;
using UserPresentaton;
using NotificationSetting = FrontendServices.Models.NotificationSetting;

namespace FrontendServices.Controllers
{
    public class DevelopersController : ApiController
    {
        private const string PageParamName = "page";
        private readonly IConfirmationService _confirmationService;
        private readonly DevelopersMapper _mapper;

        private readonly IUserManager _userManager;
        private readonly IUserPresentationProvider _userPresentationProvider;

        private readonly IPaginationWrapper<UserManagement.Domain.Account> _paginationWrapper;

        public DevelopersController(
            IUserManager userManager,
            DevelopersMapper mapper,
            IConfirmationService confirmationService,
            IUserPresentationProvider userPresentationProvider, IPaginationWrapper<Account> paginationWrapper)
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
        }

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
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
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
            Require.NotEmpty(searchString, nameof(searchString));

            var users = _userManager.GetUserList(searchString);

            var developerPageDevelopers = users
                .Where(GetAccountFilter())
                .Select(_mapper.ToDeveloperPageDeveloper);
            return developerPageDevelopers;
        }

        [HttpGet]
        [Route("developers/{id}")]
        public IHttpActionResult GetDeveloper(int id)
        {
            Require.Positive(id, nameof(id));
            try
            {
                var user = _userManager.GetUser(id);
                if (!GetAccountFilter()(user))
                {
                    throw new AccountNotFoundException();
                }

                if (HttpContext.Current.User.IsInRole(AccountRole.User))
                {
                    return Ok(_mapper.ToDeveloper(user));
                }

                return Ok(_mapper.ToGuestDeveloper(user));
            }
            catch (AccountNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("developers")]
        public IHttpActionResult RegisterNewDeveloper([FromBody] RegisterDeveloperRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                    StudyingDirection = request.Department,
                    VkProfileUri = request.VkProfileUri == null ? null : new Uri(request.VkProfileUri)
                });
            try
            {
                _userManager.CreateUser(createAccountRequest);
            }
            catch (AccountAlreadyExistsException)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Conflict));
            }

            return Ok();
        }

        [HttpPut]
        [Route("developers/{id}")]
        [Authorization(AccountRole.User)]
        public IHttpActionResult UpdateProfile(int id, [FromBody] ProfileUpdateRequest profileRequest)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(profileRequest, nameof(profileRequest));

            User.AssertResourceOwnerOrAdmin(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Account userToChange;
            try
            {
                userToChange = _userManager.GetUser(id);
            }
            catch (AccountNotFoundException)
            {
                return NotFound();
            }

            if (profileRequest != null)
            {
                var profile = new Profile
                {
                    Image = profileRequest.Image,
                    InstituteName = profileRequest.InstituteName,
                    PhoneNumber = profileRequest.InstituteName,
                    Specialization = profileRequest.Specialization,
                    StudentAccessionYear = profileRequest.StudentAccessionYear,
                    StudyingDirection = profileRequest.StudyingDirection,
                    VkProfileUri = profileRequest.VkProfileUri
                };

                userToChange.Profile = profile;
            }

            _userManager.UpdateUser(userToChange);

            return Ok();
        }

        [HttpPut]
        [Route("developers/password/{id}")]
        public IHttpActionResult ChangePassword(int id, [FromBody] string newPassword)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(newPassword, nameof(newPassword));

            User.AssertResourceOwner(id);

            Account userToChange;

            try
            {
                userToChange = _userManager.GetUser(id);
            }
            catch (AccountNotFoundException)
            {
                return NotFound();
            }

            if (newPassword != null)
            {
                userToChange.Password = new Password(newPassword);
            }

            _userManager.UpdateUser(userToChange);

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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.IsInRole(AccountRole.Administrator))
            {
                if (notificationSettings?.Any(
                    setting => setting.NotificationSettingValue == NotificationSettingValue.DontSend) ?? false)
                {
                    return BadRequest("You can't turn off notification sending");
                }
            }

            try
            {
                _userManager.GetUser(id);
            }
            catch (AccountNotFoundException)
            {
                return NotFound();
            }

            if (notificationSettings != null)
            {
                foreach (var notificationSetting in notificationSettings)
                {
                    _userPresentationProvider.UpdateNotificationSetting(
                        new UserPresentaton.NotificationSetting(
                            id,
                            notificationSetting.NotificationType,
                            notificationSetting.NotificationSettingValue));
                }
            }

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
            catch (AccountNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return
                Enum.GetValues(typeof (NotificationType))
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
            Require.NotEmpty(confirmationToken, nameof(confirmationToken));

            try
            {
                _confirmationService.ConfirmEmail(confirmationToken);
            }
            catch (TokenNotFoundException)
            {
                return BadRequest("Token not found");
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok();
        }

        private Func<Account, bool> GetAccountFilter()
        {
            if (User.IsInRole(AccountRole.Administrator))
            {
                return account => account.ConfirmationStatus == ConfirmationStatus.EmailConfirmed
                                  || account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed;
            }

            return account => account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed;
        }

        private Expression<Func<Account, bool>> GetAccountFilterExpression()
        {
            if (User.IsInRole(AccountRole.Administrator))
            {
                return account => account.ConfirmationStatus == ConfirmationStatus.EmailConfirmed
                                  || account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed;
            }

            return account => account.ConfirmationStatus == ConfirmationStatus.FullyConfirmed;
        }
    }
}