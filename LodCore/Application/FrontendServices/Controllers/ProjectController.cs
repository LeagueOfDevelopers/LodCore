using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Common;
using FrontendServices.App_Data.Authorization;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Authorization;
using FrontendServices.Models;
using Journalist;
using Journalist.Collections;
using Journalist.Extensions;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using UserManagement.Application;
using UserManagement.Domain;
using Project = ProjectManagement.Domain.Project;

namespace FrontendServices.Controllers
{
    public class ProjectController : ApiController
    {
        public ProjectController(
            IProjectProvider projectProvider, 
            ProjectsMapper projectsMapper,
            IAuthorizer authorizer, IUserManager userManager)
        {
            Require.NotNull(projectProvider, nameof(projectProvider));
            Require.NotNull(projectsMapper, nameof(projectsMapper));
            Require.NotNull(authorizer, nameof(authorizer));

            _projectProvider = projectProvider;
            _projectsMapper = projectsMapper;
            _userManager = userManager;
        }

        [Route("projects/random/{count}")]
        public IEnumerable<IndexPageProject> GetRandomIndexPageProjects(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            List<Project> requiredProjects;
            if (User.IsInRole(AccountRole.User))
            {
                requiredProjects = _projectProvider.GetProjects();
                
            }
            else
            {
                requiredProjects = _projectProvider.GetProjects(
                       project => ProjectsPolicies.OnlyDoneOrInProgress(project)
                                  && ProjectsPolicies.OnlyPublic(project));
            }

            var randomProjects = requiredProjects.GetRandom(count);

            return randomProjects.Select(_projectsMapper.ToIndexPageProject);
        }

        [HttpGet]
        [Route("projects")]
        public IEnumerable<ProjectPreview> GetAllProjects()
        {
            var paramsQuery = Request.RequestUri.Query;

            var paramsDictionary =
                paramsQuery.Split(new[] {'?'}, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(i => i.Split('=')[0], i => i.Split('=')[1]);
            
            var requiredProjects = GetSomeProjects(paramsDictionary);

            if (!User.IsInRole(AccountRole.User))
            {
                requiredProjects = requiredProjects
                    .Where(ProjectsPolicies.OnlyPublic)
                    .Where(ProjectsPolicies.OnlyDoneOrInProgress);
            }

            return requiredProjects.Select(_projectsMapper.ToProjectPreview);
        }

        [HttpPost]
        [Route("projects")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult CreateProject([FromBody]Models.CreateProjectRequest createProjectRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new ProjectManagement.Application.CreateProjectRequest(
                createProjectRequest.Name,
                createProjectRequest.ProjectTypes,
                createProjectRequest.Info,
                createProjectRequest.ProjectStatus,
                createProjectRequest.AccessLevel,
                createProjectRequest.LandingImageUri,
                createProjectRequest.Screenshots);

            _projectProvider.CreateProject(request);

            return Ok();
        }

        [HttpPost]
        [Route("projects/{projectId}/developer/{developerId}")]
        [Authorization(AccountRole.User)]
        public IHttpActionResult AddDeveloperToProject(int projectId, int developerId, [FromBody]string role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User.AssertResourceOwnerOrAdmin(developerId);
            try
            {
                _projectProvider.GetProject(projectId);
                _userManager.GetUser(developerId);
            }
            catch (ProjectNotFoundException)
            {
                return NotFound();
            }
            catch (AccountNotFoundException)
            {
                return NotFound();
            }

            try
            {
                _projectProvider.AddUserToProject(projectId, developerId, role);
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }

            return Ok();
        }

        [HttpDelete]
        [Route("projects/{projectId}/developer/{developerId}")]
        [Authorization(AccountRole.User)]
        public IHttpActionResult DeleteDeveloperFromProject(int projectId, int developerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User.AssertResourceOwnerOrAdmin(developerId);
            Project projectToDeleteUser;

            try
            {
                projectToDeleteUser = _projectProvider.GetProject(projectId);
                _userManager.GetUser(developerId);
            }
            catch (ProjectNotFoundException)
            {
                return NotFound();
            }
            catch (AccountNotFoundException)
            {
                return NotFound();
            }

            if (projectToDeleteUser.ProjectMemberships.Where(
                    membership => membership.DeveloperId == developerId)
                    .ToList().IsEmpty())
            {
                return NotFound();
            }
                
            _projectProvider.RemoveUserFromProject(projectId, developerId);

            return Ok();
        }

        [Route("projects/page/{pageNumber}")]
        public IEnumerable<ProjectPreview> GetProjectByPage(int pageNumber)
        {
            var requiredProjects = _projectProvider.GetProjects(pageNumber);

            return requiredProjects.Select(_projectsMapper.ToProjectPreview);
        }

        [HttpGet]
        [Route("projects/{projectId}")]
        public IHttpActionResult GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));

            try
            {
                var project = _projectProvider.GetProject(projectId);

                if (User.IsInRole(AccountRole.User))
                {
                    return Ok(_projectsMapper.ToAdminProject(project));
                }

                if (!ProjectsPolicies.OnlyPublic(project) || !ProjectsPolicies.OnlyDoneOrInProgress(project))
                {
                    return Unauthorized();
                }

                return Ok(_projectsMapper.ToProject(project));
            }
            catch (ProjectNotFoundException)
            {
                return NotFound();
            }
        }

        private IEnumerable<Project> GetSomeProjects(Dictionary<string, string> paramsDictionary)
        {
            string page, categoriesQuery;
            int pageNumber;

            if (paramsDictionary.TryGetValue(PageParameterName, out page))
            {
                if (!int.TryParse(page, out pageNumber))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                pageNumber = 0;
            }

            paramsDictionary.TryGetValue(CategoriesQueryParameterName, out categoriesQuery);

            var projectTypes = categoriesQuery.IsNullOrEmpty()
                ? Enum.GetValues(typeof(ProjectType)) as IEnumerable<ProjectType>
                : categoriesQuery.Split(',').Select(int.Parse).Select(category => (ProjectType)category).ToArray();


            var requiredProjects = _projectProvider.GetProjects(pageNumber,
                project => project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)))
                .OrderByDescending(project => project.ProjectTypes.Intersect(projectTypes).Count());
            
            return requiredProjects;
        } 

        private readonly IProjectProvider _projectProvider;
        private readonly ProjectsMapper _projectsMapper;
        private readonly IUserManager _userManager;

        private const string CategoriesQueryParameterName = "categories";
        private const string PageParameterName = "page";
    }
}