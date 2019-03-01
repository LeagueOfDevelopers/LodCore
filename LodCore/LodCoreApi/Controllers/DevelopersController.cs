using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Journalist;
using LodCoreApi.Mappers;
using LodCoreApi.Pagination;
using LodCoreApi.Security;
using LodCore.Domain.Exceptions;
using LodCore.Domain.UserManagement;
using LodCore.Facades;
using LodCore.QueryService.Handlers;
using LodCore.QueryService.Queries;
using LodCore.QueryService.Queries.DeveloperQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using LodCore.QueryService.Queries.ProjectQuery;

namespace LodCoreApi.Controllers
{
    [Produces("application/json")]
    public class DevelopersController : Controller
    {
        private readonly DeveloperQueryHandler _developerQueryHandler;

        public DevelopersController(
            DeveloperQueryHandler developerQueryHandler)
        {
            _developerQueryHandler = developerQueryHandler;
        }

        [HttpGet]
        [Route("developers/random/{count}")]
        public IActionResult GetRandomIndexPageDevelopers(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var result = _developerQueryHandler.Handle(new AllAccountsQuery());
            result.SelectRandomDevelopers(count, GetUserRole());
            
            return Ok(result);
        }

        [HttpGet]
        [Route("developers")]
        public IActionResult GetDevelopers(
            [FromQuery(Name = "count")] int count,
            [FromQuery(Name = "offset")] int offset)
        {
            var result = _developerQueryHandler.Handle(new GetSomeDevelopersQuery(offset, count));
            result.FilterResult(GetUserRole());
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [Route("developers/{id}")]
        public IActionResult GetDeveloper(int id)
        {
            Require.Positive(id, nameof(id));
            try
            {
                var userRole = GetUserRole();

                var result = _developerQueryHandler.Handle(new GetDeveloperQuery(id));

                if (!result.IsVisible(userRole)) return Unauthorized();
                
                    if (userRole != AccountRole.Unknown) return Ok(result);
                    else return Ok(result.GetGuestView());
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", id.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("developers/search/{searchString}")]
        public IActionResult SearchDevelopers(
            string searchString,
            [FromQuery(Name = "count")] int count,
            [FromQuery(Name = "offset")] int offset)
        {
            var result = _developerQueryHandler.Handle(new SearchDevelopersQuery(searchString));
            result.FilterResult(GetUserRole());
            result.CutOff(count, offset);

            return Ok(result);
        }

        private AccountRole GetUserRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Claims.First(claim => claim.Type == "role").Value == Claims.Roles.Admin)
                {
                    return AccountRole.Administrator;
                }
                else return AccountRole.User;
            }
            else return AccountRole.Unknown;
        }
    }
}