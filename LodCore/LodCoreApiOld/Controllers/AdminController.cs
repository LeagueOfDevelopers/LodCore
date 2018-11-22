using System;
using System.Web.Http;
using LodCoreApiOld.Authorization;
using Journalist;
using System.Text.RegularExpressions;
using Serilog;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Facades;
using LodCoreLibraryOld.Infrastructure.EventBus;
using LodCoreLibraryOld.Domain.Exceptions;
using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreApiOld.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IUserManager _userManager;
        private readonly IEventPublisher _eventPublisher;

        public AdminController(
            IConfirmationService confirmationService, 
            IUserManager userManager,
            IEventPublisher eventPublisher)
        {
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(eventPublisher, nameof(eventPublisher));

            _confirmationService = confirmationService;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
        }

        [HttpPost]
        [Route("admin/developers/confirm/{userId}")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult ConfirmDeveloper(int userId)
        {
            Require.Positive(userId, nameof(userId));
            try
            {
                _confirmationService.ConfirmProfile(userId);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", userId.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Failed to confirm user with id={0}. {1} StackTrace: {2}", userId.ToString(), ex.Message, ex.StackTrace);
                return Conflict();
            }

            return Ok();
        }

        [HttpPost]
        [Route("admin/notification")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult SendAdminNotification([FromBody] AdminNotificationInfo adminNotificationInfo)
        {
            Require.NotNull(adminNotificationInfo, nameof(adminNotificationInfo));
            _eventPublisher.PublishEvent(adminNotificationInfo);
            return Ok();
        }

        [HttpPost]
        [Route("admin/developers/{id}/hide/{condition}")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult ChangeUserHideStatus(int id, bool condition)
        {
            Require.Positive(id, nameof(id));
            var account = _userManager.GetUser(id);
            account.IsHidden = condition;
            _userManager.UpdateUser(account);
            return Ok();
        }

        [HttpPost]
        [Route("admin/developers/{id}/date/{newRegistrationDate}")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult ChangeUserRegistrationDate(int id, string newRegistrationDate)
        {
            Require.Positive(id, nameof(id));
            Require.NotEmpty(newRegistrationDate, nameof(newRegistrationDate));
            if (!DateIsCorrect(newRegistrationDate))
                return BadRequest();
            var dateComponents = newRegistrationDate.Split('.');
            var account = _userManager.GetUser(id);
            account.RegistrationTime = new DateTime(Convert.ToInt32(dateComponents[2]), Convert.ToInt32(dateComponents[1]), Convert.ToInt32(dateComponents[0]));
            _userManager.UpdateUser(account);
            return Ok();
        }

        [HttpPost]
        [Route("admin/{id}")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult PromoteToAdmin(int id)
        {
            Require.Positive(id, nameof(id));
            var account = _userManager.GetUser(id);
            account.Role = AccountRole.Administrator;
            _userManager.UpdateUser(account);
            return Ok();
        }

        private bool DateIsCorrect(string date)
        {
            var rgx = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]|(?:Jan|Mar|May|Jul|Aug|Oct|Dec)))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2|(?:Feb))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9]|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep))|(?:1[0-2]|(?:Oct|Nov|Dec)))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
            return rgx.IsMatch(date);
        }
    }
}