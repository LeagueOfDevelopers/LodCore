using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using Journalist;
using Journalist.Extensions;
using LodCoreApiOld.App_Data;
using LodCoreApiOld.App_Data.Authorization;
using LodCoreApiOld.App_Data.Mappers;
using LodCoreApiOld.Authorization;
using LodCoreApiOld.Models;
using LodCoreLibraryOld.Common;
using LodCoreLibraryOld.Domain.Exceptions;
using LodCoreLibraryOld.Domain.ProjectManagment;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Facades;
using Serilog;
using Image = LodCoreLibraryOld.Common.Image;
using Project = LodCoreLibraryOld.Domain.ProjectManagment.Project;

namespace LodCoreApiOld.Controllers
{
    public class ProjectController : ApiController
    {
        private const string CategoriesQueryParameterName = "categories";
        private const string PageParameterName = "page";
        private readonly IPaginationWrapper<Project> _paginationWrapper;

        private readonly IProjectProvider _projectProvider;
        private readonly ProjectsMapper _projectsMapper;
        private readonly IUserManager _userManager;

        public ProjectController(
            IProjectProvider projectProvider,
            ProjectsMapper projectsMapper,
            IUserManager userManager,
            IPaginationWrapper<Project> paginationWrapper)
        {
            Require.NotNull(projectProvider, nameof(projectProvider));
            Require.NotNull(projectsMapper, nameof(projectsMapper));
            Require.NotNull(userManager, nameof(userManager));
            Require.NotNull(paginationWrapper, nameof(paginationWrapper));

            _projectProvider = projectProvider;
            _projectsMapper = projectsMapper;
            _userManager = userManager;
            _paginationWrapper = paginationWrapper;
        }

        [Route("projects/random/{count}")]
        public IEnumerable<IndexPageProject> GetRandomIndexPageProjects(int count)
        {
            Require.ZeroOrGreater(count, nameof(count));

            List<Project> requiredProjects;
            if (User.IsInRole(AccountRole.User))
                requiredProjects = _projectProvider.GetProjects();
            else
                requiredProjects = _projectProvider.GetProjects(
                    project => ProjectsPolicies.OnlyDoneOrInProgress(project));

            var randomProjects = requiredProjects.GetRandom(count);

            return randomProjects.Select(_projectsMapper.ToIndexPageProject);
        }

        [HttpGet]
        [Route("projects/{projectsToSkip}/{projectsToReturn}")]
        public PaginableObject GetAllProjects(int projectsToSkip, int projectsToReturn)
        {
            var paramsQuery = Request.RequestUri.Query;

            var paramsDictionary =
                paramsQuery.Split(new[] {'?', '&'}, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(i => i.Split('=')[0], i => i.Split('=')[1]);

            var requiredProjects = GetSomeProjects(projectsToSkip, projectsToReturn, paramsDictionary);

            if (!User.IsInRole(AccountRole.User))
                requiredProjects = requiredProjects
                    .Where(ProjectsPolicies.OnlyDoneOrInProgress);

            var projecsPreviews = requiredProjects.Select(_projectsMapper.ToProjectPreview);
            return _paginationWrapper.WrapResponse(projecsPreviews,
                GetPublicProjectsCounterExpression(paramsDictionary));
        }

        [HttpPost]
        [Route("projects")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult CreateProject([FromBody] ProjectActionRequest createProjectRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

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
        [Authorization(AccountRole.User)]
        public IHttpActionResult AddDeveloperToProject(int projectId, int developerId, [FromBody] string role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            User.AssertResourceOwnerOrAdmin(developerId);
            Project project;
            Account user;
            try
            {
                project = _projectProvider.GetProject(projectId);
                user = _userManager.GetUser(developerId);
            }
            catch (ProjectNotFoundException ex)
            {
                Log.Error("Failed to get project with id={0}. {1} StackTrace: {2}", projectId.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", developerId.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }

            try
            {
                _projectProvider.AddUserToProject(projectId, developerId, role, user.Firstname, user.Lastname,
                    project.Name);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error("Failed to add user with id={0}(role={1}) to project with id={2}. {3} StackTrace: {4}",
                    developerId.ToString(), role, projectId.ToString(), ex.Message, ex.StackTrace);
                return Conflict();
            }

            return Ok();
        }

        [HttpPut]
        [Route("projects/{projectId}")]
        public IHttpActionResult UpdateProject(int projectId, [FromBody] ProjectActionRequest updateProjectRequest)
        {
            Require.Positive(projectId, nameof(projectId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var projectToUpdate = _projectProvider.GetProject(projectId);

            projectToUpdate.Info = updateProjectRequest.Info;
            projectToUpdate.Name = updateProjectRequest.Name;
            projectToUpdate.ProjectTypes = new HashSet<ProjectType>(updateProjectRequest.ProjectTypes);
            projectToUpdate.ProjectStatus = updateProjectRequest.ProjectStatus;
            projectToUpdate.LandingImage = updateProjectRequest.LandingImage;
            projectToUpdate.Screenshots = new HashSet<Image>(updateProjectRequest.Screenshots);
            projectToUpdate.Links = new HashSet<ProjectLink>(updateProjectRequest.Links);
            projectToUpdate.LinksToGithubRepositories =
                new HashSet<Uri>(updateProjectRequest.LinksToGithubRepositories);

            _projectProvider.UpdateProject(projectToUpdate);

            return Ok();
        }

        [HttpDelete]
        [Route("projects/{projectId}/developer/{developerId}")]
        [Authorization(AccountRole.User)]
        public IHttpActionResult DeleteDeveloperFromProject(int projectId, int developerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            User.AssertResourceOwnerOrAdmin(developerId);
            Project projectToDeleteUser;
            Account user;

            try
            {
                projectToDeleteUser = _projectProvider.GetProject(projectId);
                user = _userManager.GetUser(developerId);
            }
            catch (ProjectNotFoundException ex)
            {
                Log.Error("Failed to get project with id={0}. {1} StackTrace: {2}", projectId.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }
            catch (AccountNotFoundException ex)
            {
                Log.Error("Failed to get user with id={0}. {1} StackTrace: {2}", developerId.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }

            if (projectToDeleteUser.ProjectMemberships.Where(
                    membership => membership.DeveloperId == developerId)
                .ToList().IsEmpty())
                return NotFound();

            _projectProvider.RemoveUserFromProject(projectId, developerId, user.Firstname, user.Lastname,
                projectToDeleteUser.Name);

            return Ok();
        }

        [HttpGet]
        [Route("projects/{projectId}")]
        public IHttpActionResult GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));

            var issueTypes = new[] {IssueType.Research, IssueType.Task}.ToList();
            var statusOfIssues = new[] {IssueStatus.Ready, IssueStatus.InProgress}.ToList();

            try
            {
                var project = _projectProvider.GetProject(projectId, issueTypes, statusOfIssues);

                if (User.IsInRole(AccountRole.User)) return Ok(_projectsMapper.ToAdminProject(project));

                if (!ProjectsPolicies.OnlyDoneOrInProgress(project)) return Unauthorized();

                return Ok(_projectsMapper.ToProject(project));
            }
            catch (ProjectNotFoundException ex)
            {
                Log.Error("Failed to get project with id={0}. {1} StackTrace: {2}", projectId.ToString(), ex.Message,
                    ex.StackTrace);
                return NotFound();
            }
        }

        private IEnumerable<Project> GetSomeProjects(int projectsToSkip, int projectsToReturn,
            Dictionary<string, string> paramsDictionary)
        {
            string categoriesQuery;

            paramsDictionary.TryGetValue(CategoriesQueryParameterName, out categoriesQuery);

            var projectTypes = categoriesQuery.IsNullOrEmpty()
                ? Enum.GetValues(typeof(ProjectType)) as IEnumerable<ProjectType>
                : categoriesQuery.Split(',').Select(int.Parse).Select(category => (ProjectType) category).ToArray();


            var requiredProjects = _projectProvider.GetProjects(projectsToSkip, projectsToReturn,
                    project => project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)))
                .OrderByDescending(project => project.ProjectTypes.Intersect(projectTypes).Count());

            return requiredProjects;
        }

        private Expression<Func<Project, bool>> GetPublicProjectsCounterExpression(
            Dictionary<string, string> paramsDictionary)
        {
            string categoriesQuery;

            paramsDictionary.TryGetValue(CategoriesQueryParameterName, out categoriesQuery);

            var projectTypes = categoriesQuery.IsNullOrEmpty()
                ? Enum.GetValues(typeof(ProjectType)) as IEnumerable<ProjectType>
                : categoriesQuery.Split(',').Select(int.Parse).Select(category => (ProjectType) category).ToArray();

            return User.IsInRole(AccountRole.Administrator) || User.IsInRole(AccountRole.User)
                ? (Expression<Func<Project, bool>>) (project =>
                    project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)))
                : (project =>
                    project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)) &&
                    (project.ProjectStatus == ProjectStatus.Done || project.ProjectStatus == ProjectStatus.InProgress));
        }
    }
}