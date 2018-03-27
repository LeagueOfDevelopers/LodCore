using System;
using System.Net;
using System.Web.Http;
using Common;
using FrontendServices.Models;
using Journalist;
using UserManagement.Application;
using UserManagement.Domain;
using Serilog;

namespace FrontendServices.Controllers
{
    public class AuthorizationController : ApiController
    {
        private readonly IAuthorizer _authorizer;

        public AuthorizationController(IAuthorizer authorizer)
        {
            Require.NotNull(authorizer, nameof(authorizer));
            _authorizer = authorizer;
        }

        [HttpPost]
        [Route("login")]
        public AuthorizationTokenInfo Authorize([FromBody] Credentials credentials)
        {
            try
            {
                var token = _authorizer.Authorize(credentials.Email, new Password(credentials.Password));
                return token;
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error(ex.Message + "StackTrace:" + ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error(ex.Message + "StackTrace:" + ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }
    }
}