﻿using System;
using System.Web.Http;
using FrontendServices.Authorization;
using Journalist;
using NotificationService;
using UserManagement.Application;
using UserManagement.Domain;
using Common;
using Serilog;

namespace FrontendServices.Controllers
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
            try
            {
                Require.Positive(userId, nameof(userId));
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Log.Warning(ex, ex.Message);
                return BadRequest();
            }
            try
            {
                _confirmationService.ConfirmProfile(userId);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Warning(ex, ex.Message);
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning(ex, ex.Message);
                return Conflict();
            }

            return Ok();
        }

        [HttpPost]
        [Route("admin/notification")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult SendAdminNotification([FromBody] AdminNotificationInfo adminNotificationInfo)
        {
            try
            {
                Require.NotNull(adminNotificationInfo, nameof(adminNotificationInfo));
            }
            catch(NullReferenceException ex)
            {
                Log.Warning(ex, ex.Message);
                return BadRequest();
            }
            _eventPublisher.PublishEvent(adminNotificationInfo);
            return Ok();
        }

        [HttpPost]
        [Route("admin/developers/{id}/hide/{condition}")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult ChangeUserHideStatus(int id, bool condition)
        {
            try
            {
                Require.Positive(id, nameof(id));
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Log.Warning(ex, ex.Message);
                return BadRequest();
            }
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
            try
            {
                Require.Positive(id, nameof(id));
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Log.Warning(ex, ex.Message);
                return BadRequest();
            }
            var account = _userManager.GetUser(id);
            account.Role = AccountRole.Administrator;
            _userManager.UpdateUser(account);
            return Ok();
        }
    }
}