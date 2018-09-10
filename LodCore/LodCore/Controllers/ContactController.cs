using Microsoft.AspNetCore.Mvc;
using LodCore.Models;
using Journalist;
using LodCoreLibrary.Infrastructure.ContactContext;
using LodCoreLibrary.Domain.NotificationService;

namespace LodCore.Controllers
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