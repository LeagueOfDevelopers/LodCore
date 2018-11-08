using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Journalist;
using LodCore.Mappers;
using LodCore.Pagination;
using LodCoreLibrary.Domain.Exceptions;
using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.Facades;
using LodCoreLibrary.QueryService.Handlers;
using LodCoreLibrary.QueryService.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LodCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Developers")]
    public class DevelopersController : Controller
    {
        private const string PageParamName = "page";
        private readonly IConfirmationService _confirmationService;
        private readonly DevelopersMapper _mapper;

        private readonly IPaginationWrapper<Account> _paginationWrapper;
        private readonly IPasswordManager _passwordManager;

        private readonly IUserManager _userManager;
        private readonly IUserPresentationProvider _userPresentationProvider;
        private readonly DeveloperQueryHandler _developerQueryHandler;

        public DevelopersController(
            IUserManager userManager,
            DevelopersMapper mapper,
            IConfirmationService confirmationService,
            IUserPresentationProvider userPresentationProvider,
            IPaginationWrapper<Account> paginationWrapper,
            IPasswordManager passwordManager,
            DeveloperQueryHandler developerQueryHandler)
        {
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(mapper, nameof(mapper));
            Require.NotNull(confirmationService, nameof(confirmationService));
            Require.NotNull(userPresentationProvider, nameof(userPresentationProvider));

            _userManager = userManager;
            _mapper = mapper;
            _confirmationService = confirmationService;
            _userPresentationProvider = userPresentationProvider;
            _paginationWrapper = paginationWrapper;
            _passwordManager = passwordManager;
            _developerQueryHandler = developerQueryHandler;
        }

        [HttpGet]
        [Route("developers")]
        public IActionResult GetDevelopers(
            [FromQuery(Name = "count")] int count,
            [FromQuery(Name = "offset")] int offset)
        {
            var result = _developerQueryHandler.Handle(new GetSomeDevelopersQuery(offset, count));
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
                var user = _developerQueryHandler.Handle(new GetDeveloperQuery(id));
                if (!User.Identity.IsAuthenticated)
                    return Ok(user.GetGuestView());

                return Ok(user);
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", id.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }
        }
    }
}