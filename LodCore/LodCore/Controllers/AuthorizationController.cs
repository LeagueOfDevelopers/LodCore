using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LodCore.Models;
using LodCore.Security;
using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.Exceptions;
using LodCoreLibrary.Facades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LodCore.Controllers
{
    [Produces("application/json")]
    public class AuthorizationController : Controller
    {
        private readonly IJwtIssuer _jwtIssuer;
        private readonly SecuritySettings _securitySettings;
        private readonly IUserManager _userManager;

        public AuthorizationController(IJwtIssuer jwtIssuer, SecuritySettings securitySettings, IUserManager userManager)
        {
            _jwtIssuer = jwtIssuer;
            _securitySettings = securitySettings;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var account = _userManager.GetUserByCredentials(request.Email, request.Password);
                var token = _jwtIssuer.IssueJwt(account.Role.ToString(), account.UserId);
                return Ok(token);
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