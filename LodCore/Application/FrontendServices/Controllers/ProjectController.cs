using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Common;
using FrontendServices.App_Data.AuthorizationPolicies;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Models;
using Journalist;
using ProjectManagement.Application;

namespace FrontendServices.Controllers
{
    public class ProjectController : ApiController
    {
        public ProjectController(IProjectProvider projectProvider, ProjectsMapper projectsMapper)
        {
            Require.NotNull(projectProvider, nameof(projectProvider));
            Require.NotNull(projectsMapper, nameof(projectsMapper));
            
            _projectProvider = projectProvider;
            _projectsMapper = projectsMapper;
        }

        [Route("projects/random/{count}")]
        public IEnumerable<IndexPageProject> GetRandomIndexPageProjects(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var requiredProjects = _projectProvider.GetProjects(
                project => ProjectsPolicies.OnlyDoneOrInProgress(project)
                           && ProjectsPolicies.OnlyPublic(project));

            var randomProjects = requiredProjects.GetRandom(count);

            return randomProjects.Select(_projectsMapper.FromDomainEntity);
        }

        private readonly IProjectProvider _projectProvider;
        private readonly ProjectsMapper _projectsMapper;
    }
}