using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Common;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Models;
using Journalist;
using UserManagement.Application;

namespace FrontendServices.Controllers
{
    public class DevelopersController : ApiController
    {
        public DevelopersController(IUserManager userManager, DevelopersMapper mapper)
        {
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(mapper, nameof(mapper));

            _userManager = userManager;
            _mapper = mapper;
        }

        [Route("developers/random/{count}")]
        public IEnumerable<IndexPageDeveloper> GetRandomIndexPageDevelopers(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var users = _userManager.GetUserList().GetRandom(count);
            var indexPageDevelopers = users.Select(_mapper.FromDomainEntity);
            return indexPageDevelopers;
        }
         
        private readonly IUserManager _userManager;
        private readonly DevelopersMapper _mapper;
    }
}
