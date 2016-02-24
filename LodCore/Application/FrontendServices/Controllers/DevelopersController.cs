using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using Common;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Models;
using Journalist;
using Journalist.Extensions;
using UserManagement.Application;
using UserManagement.Domain;
using UserPresentaton;
using NotificationSetting = UserPresentaton.NotificationSetting;

namespace FrontendServices.Controllers
{
    public class DevelopersController : ApiController
    {
        private readonly IConfirmationService _confirmationService;
        private readonly DevelopersMapper _mapper;

        private readonly IUserManager _userManager;
        private readonly IUserPresentationProvider _userPresentationProvider;

        public DevelopersController(
            IUserManager userManager,
            DevelopersMapper mapper,
            IConfirmationService confirmationService,
            IUserPresentationProvider userPresentationProvider)
        {
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(mapper, nameof(mapper));
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(userPresentationProvider, nameof(userPresentationProvider));

            _userManager = userManager;
            _mapper = mapper;
            _confirmationService = confirmationService;
            _userPresentationProvider = userPresentationProvider;
        }

        [Route("developers/random/{count}")]
        public IEnumerable<IndexPageDeveloper> GetRandomIndexPageDevelopers(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var users = _userManager.GetUserList(
                account => account.ConfirmationStatus != ConfirmationStatus.Unconfirmed)
                .GetRandom(count);
            var indexPageDevelopers = users.Select(_mapper.ToIndexPageDeveloper);
            return indexPageDevelopers;
        }

        [HttpGet]
        [Route("developers")]
        public IEnumerable<DeveloperPageDeveloper> GetAllDevelopers()
        {
            var users = _userManager.GetUserList(
                account => account.ConfirmationStatus != ConfirmationStatus.Unconfirmed);
            var developerPageDevelopers = users.Select(_mapper.ToDeveloperPageDeveloper);
            return developerPageDevelopers;
        }

        [HttpGet]
        [Route("developers/search/{searchString}")]
        public IEnumerable<DeveloperPageDeveloper> SearchDevelopers(string searchString)
        {
            Require.NotEmpty(searchString, nameof(searchString));

            var users = _userManager.GetUserList(searchString);

            var developerPageDevelopers = users.Select(_mapper.ToDeveloperPageDeveloper);
            return developerPageDevelopers;
        }

        [HttpGet]
        [Route("developers/{id}")]
        public Developer GetDeveloper(int id)
        {
            Require.Positive(id, nameof(id));
            try
            {
                var user = _userManager.GetUser(id);
                return _mapper.ToDeveloper(user);
            }
            catch (AccountNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
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
        public IHttpActionResult UpdateProfile(int id, [FromBody] Profile profile)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(profile, nameof(profile));

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

            if (profile != null)
            {
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
        public IHttpActionResult UpdateNotificationSetiings(int id,
            [FromBody] Models.NotificationSetting[] notificationSettings)
        {
            Require.Positive(id, nameof(id));
            Require.NotNull(notificationSettings, nameof(notificationSettings));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (notificationSettings?.Any(
                setting => setting.NotificationSettingValue == NotificationSettingValue.DontSend) ?? false)
            {
                return BadRequest("You can't turn off notification sending");
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
                        new NotificationSetting(
                            id,
                            notificationSetting.NotificationType,
                            notificationSetting.NotificationSettingValue));
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("developers/notificationsettings/{id}")]
        public Models.NotificationSetting[] GetNotificationSettings(int id)
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

            return Enum.GetValues(typeof (NotificationType)).Cast<NotificationType>().Select(name => new Models.NotificationSetting
            {
                NotificationType = name, NotificationSettingValue = _userPresentationProvider.GetUserEventSettings(id, name.ToString())
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
    }
}
