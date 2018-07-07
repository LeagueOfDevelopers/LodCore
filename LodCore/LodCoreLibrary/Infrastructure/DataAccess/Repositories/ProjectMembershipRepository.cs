using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Journalist;
using LodCoreLibrary.Common;
using LodCoreLibrary.Domain.ProjectManagment;
using NHibernate.Linq;

namespace LodCoreLibrary.Infrastructure.DataAccess.Repositories
{
    public class ProjectMembershipRepository : IProjectMembershipRepostiory
    {
        private readonly IDatabaseSessionProvider _sessionProvider;

        public ProjectMembershipRepository(IDatabaseSessionProvider sessionProvider)
        {
            Require.NotNull(sessionProvider, nameof(sessionProvider));

            _sessionProvider = sessionProvider;
        }


        public IEnumerable<ProjectMembership> GetAllProjectMemberships(Expression<Func<ProjectMembership, bool>> predicate = null)
        {
            var session = _sessionProvider.GetCurrentSession();
            return predicate == null
                ? session.Query<ProjectMembership>().ToList()
                : session.Query<ProjectMembership>().Where(predicate).ToList();
        }
    }
}