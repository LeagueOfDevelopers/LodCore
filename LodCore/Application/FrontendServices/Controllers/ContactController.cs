using System.Net.Mail;
using System.Web.Http;
using ContactContext;
using ContactContext.Events;
using FrontendServices.Models;
using Journalist;

namespace FrontendServices.Controllers
{
    public class ContactController : ApiController
    {
        public ContactController(IContactsService contactsService)
        {
            Require.NotNull(contactsService, nameof(contactsService));
            _contactsService = contactsService;
        }

        [HttpPost]
        [Route("contact")]
        public IHttpActionResult SendContactMessage([FromBody]ContactMessage contactMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _contactsService.SendContactMessage(
                new NewContactMessage(
                    contactMessage.ClientName,
                    new MailAddress(contactMessage.ClientEmail), 
                    contactMessage.MessageTopic, 
                    contactMessage.MessageBody));
            return Ok();
        }

        private readonly IContactsService _contactsService;
    }
}
