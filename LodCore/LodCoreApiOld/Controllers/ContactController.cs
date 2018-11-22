using System.Web.Http;
using LodCoreApiOld.Models;
using Journalist;
using LodCoreLibraryOld.Infrastructure.ContactContext;
using LodCoreLibraryOld.Domain.NotificationService;

namespace LodCoreApiOld.Controllers
{
    public class ContactController : ApiController
    {
        private readonly IContactsService _contactsService;

        public ContactController(IContactsService contactsService)
        {
            Require.NotNull(contactsService, nameof(contactsService));
            _contactsService = contactsService;
        }

        [HttpPost]
        [Route("contact")]
        public IHttpActionResult SendContactMessage([FromBody] ContactMessage contactMessage)
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