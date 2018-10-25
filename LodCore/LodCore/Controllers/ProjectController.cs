using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Journalist;
using Journalist.Extensions;
using Image = LodCoreLibrary.Common.Image;
using Project = LodCoreLibrary.Domain.ProjectManagment.Project;
using Serilog;
using LodCoreLibrary.Facades;
using LodCoreLibrary.Domain.UserManagement;
using LodCoreLibrary.Domain.ProjectManagment;
using LodCoreLibrary.Domain.Exceptions;
using LodCoreLibrary.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using LodCore.Mappers;
using LodCore.Pagination;
using LodCore.Models;
using LodCore.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using LodCore.Security;
using LodCoreLibrary.QueryService;
using LodCoreLibrary.QueryService.Queries;
using LodCoreLibrary.QueryService.DTOs;
using LodCoreLibrary.QueryService.Handlers;
using LodCoreLibrary.QueryService.Views;

namespace LodCore.Controllers
{
    [Produces("application/json")]
    public class ProjectController : Controller
    {
        private const string CategoriesQueryParameterName = "categories";
        private const string PageParameterName = "page";

        private readonly IProjectProvider _projectProvider;
        private readonly ProjectsMapper _projectsMapper;
        private readonly IUserManager _userManager;
        private readonly IPaginationWrapper<Project> _paginationWrapper;
        private readonly ProjectQueryHandler _projectQueryHandler;

        public ProjectController(
            IProjectProvider projectProvider,
            ProjectsMapper projectsMapper,
            IUserManager userManager,
            IPaginationWrapper<Project> paginationWrapper,
            ProjectQueryHandler projectQueryHandler)
        {
            Require.NotNull(projectProvider, nameof(projectProvider));
            Require.NotNull(projectsMapper, nameof(projectsMapper));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(paginationWrapper, nameof(paginationWrapper));
            Require.NotNull(projectQueryHandler, nameof(projectQueryHandler));

            _projectProvider = projectProvider;
            _projectsMapper = projectsMapper;
            _userManager = userManager;
            _paginationWrapper = paginationWrapper;
            _projectQueryHandler = projectQueryHandler;
        }

        [HttpGet]
        [Route("projects/random/{count}")]
        public IEnumerable<IndexPageProject> GetRandomIndexPageProjects(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));
            
            List<Project> requiredProjects;
            if (Request.IsInRole(Claims.Roles.User))
            {
                requiredProjects = _projectProvider.GetProjects();
            }
            else
            {
                requiredProjects = _projectProvider.GetProjects(
                    project => ProjectsPolicies.OnlyDoneOrInProgress(project));
            }

            var randomProjects = requiredProjects.GetRandom(count);

            return randomProjects.Select(_projectsMapper.ToIndexPageProject);
        }

        [HttpGet]
        [Route("projects/{projectsToSkip}/{projectsToReturn}")]
        [SwaggerResponse(200, Type = typeof(AllProjectsView))]
        public IActionResult GetAllProjects(int projectsToSkip, int projectsToReturn)
        {
           var result = _projectQueryHandler.Handle(new GetProjectQuery(2));
            //var paramsQuery = Request.RequestUri.Query;
            /*var paramsQuery = Request.GetDisplayUrl();

            var paramsDictionary =
                paramsQuery.Split(new[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(i => i.Split('=')[0], i => i.Split('=')[1]);

            var requiredProjects = GetSomeProjects(projectsToSkip, projectsToReturn, paramsDictionary);

            if (!Request.IsInRole(Claims.Roles.User))
            {
                requiredProjects = requiredProjects
                    .Where(ProjectsPolicies.OnlyDoneOrInProgress);
            }

            var projecsPreviews = requiredProjects.Select(_projectsMapper.ToProjectPreview);
            //return _paginationWrapper.WrapResponse(projecsPreviews, GetPublicProjectsCounterExpression(paramsDictionary));*/
            return Ok(result);
        }

        [HttpPost]
        [Route("projects")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult CreateProject([FromBody] ProjectActionRequest createProjectRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new CreateProjectRequest(
                createProjectRequest.Name,
                createProjectRequest.ProjectTypes,
                createProjectRequest.Info,
                createProjectRequest.ProjectStatus,
                createProjectRequest.LandingImage,
                createProjectRequest.Screenshots,
                createProjectRequest.Links,
                createProjectRequest.LinksToGithubRepositories);

            var projectId = _projectProvider.CreateProject(request);

            return Ok(projectId);
        }

        [HttpPost]
        [Route("projects/{projectId}/developer/{developerId}")]
        //[Authorization(AccountRole.User]
        [Authorize]
        public IActionResult AddDeveloperToProject(int projectId, int developerId, [FromBody] string role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //User.AssertResourceOwnerOrAdmin(developerId);
            Project project;
            Account user;
            try
            {
                project = _projectProvider.GetProject(projectId);
                user = _userManager.GetUser(developerId);
            }
            catch (ProjectNotFoundException ex)
            {
                Log.Error("Failed to get project with id={0}. {1} StackTrace: {2}", projectId.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", developerId.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }

            try
            {
                _projectProvider.AddUserToProject(projectId, developerId, role, user.Firstname, user.Lastname, project.Name);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Failed to add user with id={0}(role={1}) to project with id={2}. {3} StackTrace: {4}",
                    developerId.ToString(), role, projectId.ToString(), ex.Message, ex.StackTrace);
                //return Conflict;
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut]
        [Route("projects/{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] ProjectActionRequest updateProjectRequest)
        {
            Require.Positive(projectId, nameof(projectId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectToUpdate = _projectProvider.GetProject(projectId);

            projectToUpdate.Info = updateProjectRequest.Info;
            projectToUpdate.Name = updateProjectRequest.Name;
            projectToUpdate.ProjectTypes = new HashSet<ProjectType>(updateProjectRequest.ProjectTypes);
            projectToUpdate.ProjectStatus = updateProjectRequest.ProjectStatus;
            projectToUpdate.LandingImage = updateProjectRequest.LandingImage;
            projectToUpdate.Screenshots = new HashSet<Image>(updateProjectRequest.Screenshots);
            projectToUpdate.Links = new HashSet<ProjectLink>(updateProjectRequest.Links);
            projectToUpdate.LinksToGithubRepositories = new HashSet<Uri>(updateProjectRequest.LinksToGithubRepositories);

            _projectProvider.UpdateProject(projectToUpdate);

            return Ok();
        }

        [HttpDelete]
        [Route("projects/{projectId}/developer/{developerId}")]
        //[Authorization(AccountRole.User)]
        [Authorize]
        public IActionResult DeleteDeveloperFromProject(int projectId, int developerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //User.AssertResourceOwnerOrAdmin(developerId);
            Project projectToDeleteUser;
            Account user;

            try
            {
                projectToDeleteUser = _projectProvider.GetProject(projectId);
                user = _userManager.GetUser(developerId);
            }
            catch (ProjectNotFoundException ex)
            {
                Log.Error("Failed to get project with id={0}. {1} StackTrace: {2}", projectId.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", developerId.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }

            if (projectToDeleteUser.ProjectMemberships.Where(
                membership => membership.DeveloperId == developerId)
                .ToList().IsEmpty())
            {
                return NotFound();
            }

            _projectProvider.RemoveUserFromProject(projectId, developerId, user.Firstname, user.Lastname, projectToDeleteUser.Name);

            return Ok();
        }

        [HttpGet]
        [Route("projects/{projectId}")]
        public IActionResult GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));

            var issueTypes = new[] { IssueType.Research, IssueType.Task }.ToList();
            var statusOfIssues = new[] { IssueStatus.Ready, IssueStatus.InProgress }.ToList();

            try
            {
                var project = _projectProvider.GetProject(projectId, issueTypes, statusOfIssues);

                if (Request.IsInRole(Claims.Roles.User))
                {
                    return Ok(_projectsMapper.ToAdminProject(project));
                }

                if (!ProjectsPolicies.OnlyDoneOrInProgress(project))
                {
                    return Unauthorized();
                }

                return Ok(_projectsMapper.ToProject(project));
            }
            catch (ProjectNotFoundException ex)
            {
                Log.Error("Failed to get project with id={0}. {1} StackTrace: {2}", projectId.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }
        }

        private IEnumerable<Project> GetSomeProjects(int projectsToSkip, int projectsToReturn, Dictionary<string, string> paramsDictionary)
        {
            string categoriesQuery;

            paramsDictionary.TryGetValue(CategoriesQueryParameterName, out categoriesQuery);

            var projectTypes = categoriesQuery.IsNullOrEmpty()
                ? Enum.GetValues(typeof(ProjectType)) as IEnumerable<ProjectType>
                : categoriesQuery.Split(',').Select(int.Parse).Select(category => (ProjectType)category).ToArray();


            var requiredProjects = _projectProvider.GetProjects(projectsToSkip, projectsToReturn,
                project => project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)))
                .OrderByDescending(project => project.ProjectTypes.Intersect(projectTypes).Count());

            return requiredProjects;
        }

        private Expression<Func<Project, bool>> GetPublicProjectsCounterExpression(Dictionary<string, string> paramsDictionary)
        {
            string categoriesQuery;

            paramsDictionary.TryGetValue(CategoriesQueryParameterName, out categoriesQuery);

            var projectTypes = categoriesQuery.IsNullOrEmpty()
                ? Enum.GetValues(typeof(ProjectType)) as IEnumerable<ProjectType>
                : categoriesQuery.Split(',').Select(int.Parse).Select(category => (ProjectType)category).ToArray();

            return Request.IsInRole(Claims.Roles.Admin) || Request.IsInRole(Claims.Roles.User)
                ? (Expression<Func<Project, bool>>)(project =>
                   project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)))
                : (project =>
                    project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)) &&
                    (project.ProjectStatus == ProjectStatus.Done || project.ProjectStatus == ProjectStatus.InProgress));
        }
    }
}