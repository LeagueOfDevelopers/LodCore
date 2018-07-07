using System.Web.Http;
using LodCoreApi.Models;
using Journalist;
using LodCoreLibrary.Infrastructure.ContactContext;
using LodCoreLibrary.Domain.NotificationService;

namespace LodCoreApi.Controllers
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