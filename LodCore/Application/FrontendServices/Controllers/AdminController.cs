using System;
using System.Web.Http;
using Journalist;
using UserManagement.Application;
using UserManagement.Domain;

namespace FrontendServices.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IConfirmationService _confirmationService;

        public AdminController(IConfirmationService confirmationService)
        {
            Require.NotNull(confirmationService, nameof(confirmationService));
            _confirmationService = confirmationService;
        }

        [HttpPost]
        [Route("admin/developers/confirm/{userId}")]
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
    }
}