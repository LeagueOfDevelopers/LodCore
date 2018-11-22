using Microsoft.AspNetCore.Mvc;
using LodCoreApi.Models;
using Journalist;
using LodCore.Infrastructure.ContactContext;
using LodCore.Domain.NotificationService;

namespace LodCoreApi.Controllers
{
    [Produces("application/json")]
    public class ContactController : Controller
    {
        private readonly IContactsService _contactsService;

        public ContactController(IContactsService contactsService)
        {
            Require.NotNull(contactsService, nameof(contactsService));
            _contactsService = contactsService;
        }

        [HttpPost]
        [Route("contact")]
        public IActionResult SendContactMessage([FromBody] ContactMessage contactMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _contactsService.SendContactMessage(
                new NewContactMessage(
                    contactMessage.ClientName,
                    contactMessage.ClientEmail,
                    contactMessage.MessageTopic,
                    contactMessage.MessageBody));
            return Ok();
        }
    }
}