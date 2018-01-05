using System;
using System.Web.Http;
using FrontendServices.Authorization;
using Journalist;
using NotificationService;
using UserManagement.Application;
using UserManagement.Domain;
using Common;

namespace FrontendServices.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IConfirmationService _confirmationService;
        private readonly IUserManager _userManager;
        private readonly IEventPublisher _eventPublisher;

        public AdminController(
            IConfirmationService confirmationService, 
            NotificationEventSink notificationEventSink, 
            IUserManager userManager,
            IEventPublisher eventPublisher)
        {
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(notificationEventSink, nameof(notificationEventSink));
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
            catch (AccountNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException)
            {
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