using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using Common;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Models;
using Journalist;
using UserManagement.Application;
using UserManagement.Domain;

namespace FrontendServices.Controllers
{
    public class DevelopersController : ApiController
    {
        public DevelopersController(
            IUserManager userManager, 
            DevelopersMapper mapper, 
            IConfirmationService confirmationService)
        {
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(mapper, nameof(mapper));
            Require.NotNull(confirmationService, nameof(confirmationService));

            _userManager = userManager;
            _mapper = mapper;
            _confirmationService = confirmationService;
        }

        [Route("developers/random/{count}")]
        public IEnumerable<IndexPageDeveloper> GetRandomIndexPageDevelopers(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var users = _userManager.GetUserList().GetRandom(count);
            var indexPageDevelopers = users.Select(_mapper.ToIndexPageDeveloper);
            return indexPageDevelopers;
        }
        
        [HttpGet]
        [Route("developers")]
        public IEnumerable<DeveloperPageDeveloper> GetAllDevelopers()
        {
            var users = _userManager.GetUserList(account => account.ConfirmationStatus != ConfirmationStatus.Unconfirmed);
            var developerPageDevelopers = users.Select(_mapper.ToDeveloperPageDeveloper);
            return developerPageDevelopers;
        }

        [HttpPost]
        [Route("developers")]
        public IHttpActionResult RegisterNewDeveloper([FromBody] RegisterDeveloperRequest request)
        {
            var createAccountRequest = new CreateAccountRequest(
                new MailAddress(request.Email), 
                request.LastName,
                request.FirstName,
                request.Password,
                new Profile(
                    null, 
                    null, 
                    new MailAddress(request.Email), 
                    DateTime.Now, 
                    new Uri(request.VkProfileUri),
                    request.PhoneNumber, 
                    request.AccessionYear,
                    request.Department,
                    request.InstituteName, 
                    request.StudyingProfile));
            try
            {
                _userManager.CreateUser(createAccountRequest);
            }
            catch (AccountAlreadyExistsException)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Conflict));
            }

            return Ok();
        }

        [HttpPost]
        [Route("developers/confirmation/{confirmationToken}")]
        public IHttpActionResult ConfirmEmail(string confirmationToken)
        {
            Require.NotEmpty(confirmationToken, nameof(confirmationToken));

            try
            {
                _confirmationService.ConfirmEmail(confirmationToken);
            }
            catch (TokenNotFoundException)
            {
                return BadRequest("Token not found");
            }

            return Ok();
        }

        private readonly IUserManager _userManager;
        private readonly IConfirmationService _confirmationService;
        private readonly DevelopersMapper _mapper;
    }
}
