using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;
using Journalist.Extensions;
using NHibernate.Linq;
using NotificationService;
using ProjectManagement.Domain;
using ProjectManagement.Infrastructure;

namespace DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository, IProjectRelativesRepository
    {
        private readonly DatabaseSessionProvider _databaseSessionProvider;

        public ProjectRepository(DatabaseSessionProvider databaseSessionProvider)
        {
            Require.NotNull(databaseSessionProvider, nameof(databaseSessionProvider));

            _databaseSessionProvider = databaseSessionProvider;
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

            var allProjects = criteria == null
                ? session.Query<Project>()
                : session.Query<Project>().Where(criteria);
            return allProjects.ToArray();
        }

        public Project[] GetSomeProjects(int skipCount, int takeCount)
        {
            var session = _databaseSessionProvider.GetCurrentSession();
            var requiredProjects = session.QueryOver<Project>().Skip(skipCount).Take(takeCount).List();
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
            Require.NotNull(project, nameof(project));

            var session = _databaseSessionProvider.GetCurrentSession();
            return (int) session.Save(project);
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