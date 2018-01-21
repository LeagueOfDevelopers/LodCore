using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Http;
using Common;
using FrontendServices.App_Data;
using FrontendServices.App_Data.Authorization;
using FrontendServices.App_Data.Mappers;
using FrontendServices.Authorization;
using FrontendServices.Models;
using Journalist;
using Journalist.Extensions;
using ProjectManagement;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using Serilog;
using UserManagement.Application;
using UserManagement.Domain;
using Image = Common.Image;
using Project = ProjectManagement.Domain.Project;
using ProjectActionRequest = FrontendServices.Models.ProjectActionRequest;

namespace FrontendServices.Controllers
{
    public class ProjectController : ApiController
    {
        private const string CategoriesQueryParameterName = "categories";
        private const string PageParameterName = "page";

        private readonly IProjectProvider _projectProvider;
        private readonly ProjectsMapper _projectsMapper;
        private readonly IUserManager _userManager;
        private readonly IPaginationWrapper<Project> _paginationWrapper; 

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
        public PaginableObject GetAllProjects()
        {
            var paramsQuery = Request.RequestUri.Query;

            var paramsDictionary =
                paramsQuery.Split(new[] {'?', '&'}, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(i => i.Split('=')[0], i => i.Split('=')[1]);

            var requiredProjects = GetSomeProjects(paramsDictionary);

            if (!User.IsInRole(AccountRole.User))
            {
                requiredProjects = requiredProjects
                    .Where(ProjectsPolicies.OnlyPublic)
                    .Where(ProjectsPolicies.OnlyDoneOrInProgress);
            }

            var projecsPreviews = requiredProjects.Select(_projectsMapper.ToProjectPreview);
            return _paginationWrapper.WrapResponse(projecsPreviews, GetPublicProjectsCounterExpression(paramsDictionary));
        }

        [HttpPost]
        [Route("projects")]
        [Authorization(AccountRole.Administrator)]
        public IHttpActionResult CreateProject([FromBody] ProjectActionRequest createProjectRequest)
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
                createProjectRequest.AccessLevel,
                createProjectRequest.LandingImage,
                createProjectRequest.Screenshots);

            var projectId = _projectProvider.CreateProject(request);

            return Ok(projectId);
        }

        [HttpPost]
        [Route("projects/{projectId}/developer/{developerId}")]
        [Authorization(AccountRole.User)]
        public IHttpActionResult AddDeveloperToProject(int projectId, int developerId, [FromBody] string role)
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

        [HttpPut]
        [Route("projects/{projectId}")]
        public IHttpActionResult UpdateProject(int projectId, [FromBody] ProjectActionRequest updateProjectRequest)
        {
            Require.Positive(projectId, nameof(projectId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectToUpdate = _projectProvider.GetProject(projectId);

            projectToUpdate.Info = updateProjectRequest.Info;
            projectToUpdate.AccessLevel = updateProjectRequest.AccessLevel;
            projectToUpdate.Name = updateProjectRequest.Name;
            projectToUpdate.ProjectTypes = new HashSet<ProjectType>(updateProjectRequest.ProjectTypes);
            projectToUpdate.ProjectStatus = updateProjectRequest.ProjectStatus;
            projectToUpdate.LandingImage = updateProjectRequest.LandingImage;
            projectToUpdate.Screenshots = new HashSet<Image>(updateProjectRequest.Screenshots);

            _projectProvider.UpdateProject(projectToUpdate);

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
        public PaginableObject GetProjectByPage(int pageNumber)
        {
            var requiredProjects = _projectProvider.GetProjects(pageNumber);

            var projectsPreview = requiredProjects.Select(_projectsMapper.ToProjectPreview);

            return _paginationWrapper.WrapResponse(projectsPreview);
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
            int pageNumber = 0;

            if (paramsDictionary.TryGetValue(PageParameterName, out page))
            {
                if (!int.TryParse(page, out pageNumber))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            paramsDictionary.TryGetValue(CategoriesQueryParameterName, out categoriesQuery);

            var projectTypes = categoriesQuery.IsNullOrEmpty()
                ? Enum.GetValues(typeof (ProjectType)) as IEnumerable<ProjectType>
                : categoriesQuery.Split(',').Select(int.Parse).Select(category => (ProjectType) category).ToArray();


            var requiredProjects = _projectProvider.GetProjects(pageNumber,
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

            return User.IsInRole(AccountRole.Administrator) || User.IsInRole(AccountRole.User)
                ? (Expression<Func<Project, bool>>) (project =>
                    project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)))
                : (project =>
                    project.ProjectTypes.Any(projectType => projectTypes.Contains(projectType)) &&
                    project.AccessLevel == AccessLevel.Public && (project.ProjectStatus == ProjectStatus.Done
                                                                  || project.ProjectStatus == ProjectStatus.InProgress));
        } 
    }
}