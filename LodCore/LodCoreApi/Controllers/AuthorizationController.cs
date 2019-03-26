using System;
using LodCore.Domain.Exceptions;
using LodCore.Facades;
using LodCoreApi.Models.AuthorizationModels;
using LodCoreApi.Security;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LodCoreApi.Controllers
{
    [Produces("application/json")]
    public class AuthorizationController : Controller
    {
        private readonly IJwtIssuer _jwtIssuer;
        private readonly SecuritySettings _securitySettings;
        private readonly IUserManager _userManager;

        public AuthorizationController(IJwtIssuer jwtIssuer, SecuritySettings securitySettings,
            IUserManager userManager)
        {
            _jwtIssuer = jwtIssuer;
            _securitySettings = securitySettings;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(LoginResponseModel), 200)]
        public IActionResult Login([FromBody] LoginRequestModel request)
        {
            try
            {
                var account = _userManager.GetUserByCredentials(request.Email, request.Password);
                var token = _jwtIssuer.IssueJwt(account.Role.ToString(), account.UserId);

                var response = new LoginResponseModel(token);

                return Ok(response);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with email={0}. {1} StackTrace: {2}", request.Email, ex.Message, ex.StackTrace);
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error("Failed to allow access to user with email={0}. {1} StackTrace: {2}", request.Email, ex.Message, ex.StackTrace);
                return Unauthorized();
            }
        }
    }
}