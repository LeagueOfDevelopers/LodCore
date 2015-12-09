using System;
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
                .ProjectDevelopers
                .SelectToArray(developer => developer.DeveloperId);
        }

        public Project[] GetAllProjects(Func<Project, bool> criteria = null)
        {
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return criteria == null
                    ? session.Query<Project>().ToArray()
                    : session.Query<Project>().Where(criteria).ToArray();
            }
        }

        public Project GetProject(int projectId)
        {
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return session.Get<Project>(projectId);
            }
        }

        public int SaveProject(Project project)
        {
            Require.NotNull(project, nameof(project));
            using (var session = _databaseSessionProvider.OpenSession())
            {
                return (int) session.Save(project);
            }
        }

        public void UpdateProject(Project project)
        {
            Require.NotNull(project, nameof(project));

            using (var session = _databaseSessionProvider.OpenSession())
            {
                session.Update(project);
            }
        }
    }
}