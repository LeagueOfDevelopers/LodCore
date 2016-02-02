using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Common;
using FrontendServices.App_Data.Authorization;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Models;
using Journalist;
using Journalist.Extensions;
using ProjectManagement.Application;
using ProjectManagement.Domain;
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
            var categoriesQuery = Request.RequestUri.Query;
            
            var requiredProjects = categoriesQuery.IsEmpty() 
                ? _projectProvider.GetProjects()
                : GetProjectsByCategory(categoriesQuery);

            return requiredProjects.Select(_projectsMapper.ToProjectPreview);
        }

        [Route("projects/page/{pageNumber}")]
        public IEnumerable<ProjectPreview> GetProjectByPage(int pageNumber)
        {
            var requiredProjects = _projectProvider.GetProjects(pageNumber);

            return requiredProjects.Select(_projectsMapper.ToProjectPreview);
        }

        [Route("projects/{projectId}")]
        public AdminProject GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));

            try
            {
                var project = _projectProvider.GetProject(projectId);
                return _projectsMapper.ToAdminProject(project);
            }
            catch (ProjectNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        private IEnumerable<Project> GetProjectsByCategory(string categoriesQuery)
        {
            var categories = categoriesQuery
                .Replace("?" + CategoriesQueryParameterName + "=", string.Empty)
                .Split(',')
                .Select(int.Parse);
            var projectTypes = categories.Select(category => (ProjectType)category);
            var requiredProjects = _projectProvider.GetProjects(
                project => project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)))
                .OrderByDescending(project => project.ProjectTypes.Intersect(projectTypes).Count());
            return requiredProjects;
        } 

        private readonly IProjectProvider _projectProvider;
        private readonly ProjectsMapper _projectsMapper;
        private readonly IAuthorizer _authorizer;

        private const string CategoriesQueryParameterName = "categories";
    }
}