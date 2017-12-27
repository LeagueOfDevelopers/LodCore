using System;
using System.Web.Http;
using FrontendServices.Authorization;
using Journalist;
using NotificationService;
using UserManagement.Application;
using UserManagement.Domain;
using RabbitMQEventBus;

namespace FrontendServices.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IConfirmationService _confirmationService;
        private readonly NotificationEventSink _notificationEventSink;
        private readonly IUserManager _userManager;
        private readonly IEventBus _eventBus;

        public AdminController(
            IConfirmationService confirmationService, 
            NotificationEventSink notificationEventSink, 
            IUserManager userManager,
            IEventBus eventBus)
        {
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(notificationEventSink, nameof(notificationEventSink));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(eventBus, nameof(eventBus));

            _confirmationService = confirmationService;
            _notificationEventSink = notificationEventSink;
            _userManager = userManager;
            _eventBus = eventBus;
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
            catch (AccountNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("admin/notification")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult SendAdminNotification([FromBody] AdminNotificationInfo adminNotificationInfo)
        {
            Require.NotNull(adminNotificationInfo, nameof(adminNotificationInfo));

            _eventBus.PublishEvent("Notification", "admin_notification_info", adminNotificationInfo);

            return Ok();
        }

        [HttpPost]
        [Route("admin/developers/{id}/hide/{condition}")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult ChangeUserHideStatus(int id, bool condition)
        {
            Require.Positive(id, "id");

            var account = _userManager.GetUser(id);
            account.IsHidden = condition;
            _userManager.UpdateUser(account);

            return Ok();
        }

        [HttpPost]
        [Route("admin/{id}")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult PromoteToAdmin(int id)
        {
            Require.Positive(id, "id");

            var account = _userManager.GetUser(id);
            account.Role = AccountRole.Administrator;
            _userManager.UpdateUser(account);

            return Ok();
        }
    }
}