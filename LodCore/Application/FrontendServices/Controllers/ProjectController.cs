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
using UserManagement.Application;

namespace FrontendServices.Controllers
{
    public class ProjectController : ApiController
    {
        public ProjectController(
            IProjectProvider projectProvider, 
            ProjectsMapper projectsMapper,
            IAuthorizer authorizer)
        {
            Require.NotNull(projectProvider, nameof(projectProvider));
            Require.NotNull(projectsMapper, nameof(projectsMapper));
            Require.NotNull(authorizer, nameof(authorizer));

            _projectProvider = projectProvider;
            _projectsMapper = projectsMapper;
            _authorizer = authorizer;
        }

        [Route("projects/random/{count}")]
        public IEnumerable<IndexPageProject> GetRandomIndexPageProjects(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            var requiredProjects = _projectProvider.GetProjects(
                project => ProjectsPolicies.OnlyDoneOrInProgress(project)
                           && ProjectsPolicies.OnlyPublic(project));

            var randomProjects = requiredProjects.GetRandom(count);

            return randomProjects.Select(_projectsMapper.ToIndexPageProject);
        }

        [Route("projects")]
        public IEnumerable<ProjectPreview> GetAllProjects()
        {
            var requiredProjects = _projectProvider.GetProjects();

            return requiredProjects.Select(_projectsMapper.ToProjectPreview);
        }

        private readonly IProjectProvider _projectProvider;
        private readonly ProjectsMapper _projectsMapper;
        private readonly IAuthorizer _authorizer;
    }
}