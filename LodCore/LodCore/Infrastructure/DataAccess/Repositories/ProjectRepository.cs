using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Journalist;
using Journalist.Extensions;
using LodCore.Domain.ProjectManagment;
using System.Data.SqlClient;
using Dapper;
using LodCore.QueryService.DTOs;
using LodCore.QueryService.Handlers;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository, IProjectRelativesRepository
    {
        private readonly string _connectionString;
        private readonly ProjectQueryHandler _projectQueryHandler;

        public ProjectRepository(string connectionString, ProjectQueryHandler projectQueryHandler)
        {
            _connectionString = connectionString;
            _projectQueryHandler = projectQueryHandler;
        }
        
        public int[] GetAllProjectRelativeIds(int projectId)
        {
            return GetProject(projectId)
                .ProjectMemberships
                .SelectToArray(developer => developer.DeveloperId);
        }
        
        public Project[] GetAllProjects(Func<Project, bool> criteria = null)
        {
            /*
            var session = _databaseSessionProvider.GetCurrentSession();

            IEnumerable<Project> allProjects;
            if (criteria == null) allProjects = session.Query<Project>();
            else allProjects = session.Query<Project>().Where(criteria);
            return allProjects.ToArray();*/
            return null;
        }

        public Project[] GetSomeProjects(
            int skipCount, 
            int takeCount, 
            Expression<Func<Project, int>> orderer, 
            Expression<Func<Project, bool>> predicate = null)
        {
            /*
            var session = _databaseSessionProvider.GetCurrentSession();
            IQueryable<Project> requiredProjects;
            var query = session.Query<Project>().OrderBy(orderer);
            if (predicate == null) requiredProjects = query.Skip(skipCount).Take(takeCount);
            else
                requiredProjects =
                    query.Where(predicate).Skip(skipCount).Take(takeCount);
            return requiredProjects.ToArray();*/
            return null;
        }

        public Project GetProject(int projectId)
        {
            /*
            Require.Positive(projectId, nameof(projectId));

            var session = _databaseSessionProvider.GetCurrentSession();
            return session.Get<Project>(projectId);*/
            return null;
        }

        public int SaveProject(Project project)
        {
            int projectId;

            var sqlForGettingId = "INSERT INTO projects (name, info, projectstatus, bigphotouri, smallphotouri) " +
                    "VALUES(@Name, @Info, @ProjectStatus, @BigPhotoUri, @SmallPhotoUri); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";
            var sqlForScreenshots = "INSERT INTO screenshots (projectId, bigphotouri, smallphotouri) " +
                        "VALUES(@ProjectId, @BigPhotoUri, @SmallPhotoUri);";
            var sqlForTypes = "INSERT INTO projectTypes (projectId, type) VALUES(@ProjectId, @Type);";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    projectId = connection.Query<int>(sqlForGettingId,
                                new
                                {
                                    project.Name,
                                    project.Info,
                                    project.ProjectStatus,
                                    BigPhotoUri = project.LandingImage.BigPhotoUri.ToString(),
                                    SmallPhotoUri = project.LandingImage.SmallPhotoUri.ToString()
                                },
                                transaction).FirstOrDefault();

                    project.Screenshots.ToList().ForEach(s => connection.Execute(sqlForScreenshots,
                        new
                        {
                            ProjectId = projectId,
                            BigPhotoUri = s.BigPhotoUri.ToString(),
                            SmallPhotoUri = s.SmallPhotoUri.ToString()
                        }, transaction));

                    project.ProjectTypes.ToList().ForEach(t => connection.Execute(sqlForTypes,
                        new { ProjectId = projectId, Type = t }, transaction));

                    transaction.Commit();
                }
                connection.Close();
            }

            return projectId;
        }

        public void UpdateProject(Project project)
        {
            var projectDto = new ProjectDto();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction sqlTransaction = connection.BeginTransaction();

                var sqlQuery = $"UPDATE projects SET name={projectDto.Name}, info={projectDto.Info} WHERE projectId={projectDto.ProjectId};";
                connection.Execute(sqlQuery, projectDto, sqlTransaction);

                sqlTransaction.Commit();
                connection.Close();
            }
        }

        public IEnumerable<string> GetUserRoles(int userId )
        {
            /*
            Require.Positive(userId, nameof(userId));

            var session = _databaseSessionProvider.GetCurrentSession();
            var allProjects = session.Query<Project>();
            var allRoles = allProjects
                .SelectMany(project => project.ProjectMemberships)
                .Where(membership => membership.DeveloperId == userId)
                .Select(membership => membership.Role);
            return allRoles.ToArray();*/
            return null;
        }
    }
}