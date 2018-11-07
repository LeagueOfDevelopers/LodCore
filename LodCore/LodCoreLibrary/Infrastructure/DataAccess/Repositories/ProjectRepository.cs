using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Journalist;
using Journalist.Extensions;
using NHibernate.Linq;
using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.ProjectManagment;
using System.Data.SqlClient;
using Dapper;
using LodCoreLibrary.QueryService.DTOs;

namespace LodCoreLibrary.Infrastructure.DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository, IProjectRelativesRepository
    {
        private readonly string _connectionString;
        private readonly IDatabaseSessionProvider _databaseSessionProvider;

        public ProjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public int[] GetAllProjectRelativeIds(int projectId)
        {
            return GetProject(projectId)
                .ProjectMemberships
                .SelectToArray(developer => developer.DeveloperId);
        }
        
        public Project[] GetAllProjects(Func<Project, bool> criteria = null)
        {
            var session = _databaseSessionProvider.GetCurrentSession();

            IEnumerable<Project> allProjects;
            if (criteria == null) allProjects = session.Query<Project>();
            else allProjects = session.Query<Project>().Where(criteria);
            return allProjects.ToArray();
        }

        public Project[] GetSomeProjects(
            int skipCount, 
            int takeCount, 
            Expression<Func<Project, int>> orderer, 
            Expression<Func<Project, bool>> predicate = null)
        {
            var session = _databaseSessionProvider.GetCurrentSession();
            IQueryable<Project> requiredProjects;
            var query = session.Query<Project>().OrderBy(orderer);
            if (predicate == null) requiredProjects = query.Skip(skipCount).Take(takeCount);
            else
                requiredProjects =
                    query.Where(predicate).Skip(skipCount).Take(takeCount);
            return requiredProjects.ToArray();
        }

        public Project GetProject(int projectId)
        {
            Require.Positive(projectId, nameof(projectId));

            var session = _databaseSessionProvider.GetCurrentSession();
            return session.Get<Project>(projectId);
        }

        public int SaveProject(Project project)
        {
            int projectId;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction sqlTransaction = connection.BeginTransaction();

                var sqlQuery = "INSERT INTO projects (name, info, projectstatus) " +
                    "VALUES(@Name, @Info, @ProjectStatus); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

                projectId = connection.Query<int>(sqlQuery, project, sqlTransaction).FirstOrDefault();

                sqlTransaction.Commit();
                connection.Close();
            }

            return projectId;
        }

        public void UpdateProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            var session = _databaseSessionProvider.GetCurrentSession();

            session.Update(project);
        }

        public IEnumerable<string> GetUserRoles(int userId )
        {
            Require.Positive(userId, nameof(userId));

            var session = _databaseSessionProvider.GetCurrentSession();
            var allProjects = session.Query<Project>();
            var allRoles = allProjects
                .SelectMany(project => project.ProjectMemberships)
                .Where(membership => membership.DeveloperId == userId)
                .Select(membership => membership.Role);
            return allRoles.ToArray();
        }
    }
}