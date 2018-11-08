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
        [Authorize]
        [AllowAnonymous]
        [Route("projects")]
        [SwaggerResponse(200, Type = typeof(SomeProjectsView))]
        public IActionResult GetAllProjects(
            [FromQuery(Name = "count")] int count, 
            [FromQuery(Name = "offset")] int offset,
            [FromQuery(Name = "category")] int[] categories)
        {
            var resultOfQuery = _projectQueryHandler.Handle(new GetSomeProjectsQuery(offset, count, categories));

            if (!User.Identity.IsAuthenticated)
            {
                resultOfQuery.FilterResult();
            }
            
            return Ok(resultOfQuery);
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
        [Authorize]
        [AllowAnonymous]
        [Route("projects/{projectId}")]
        public IActionResult GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));
            
            try
            {
                var project = _projectQueryHandler.Handle(new GetProjectQuery(projectId));
                
                if (!User.Identity.IsAuthenticated)
                {
                    if (project.ProjectStatus == ProjectStatus.Done || project.ProjectStatus == ProjectStatus.InProgress)
                        return Ok(project);
                    else
                        return Unauthorized();
                }
                return Ok(project);
            }
            catch (ProjectNotFoundException ex)
            {
                Log.Error("Failed to get project with id={0}. {1} StackTrace: {2}", projectId.ToString(), ex.Message, ex.StackTrace);
                return NotFound();
            }
        }
    }
}