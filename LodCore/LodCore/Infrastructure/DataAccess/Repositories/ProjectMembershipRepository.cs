using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCore.Domain.ProjectManagment;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class ProjectMembershipRepository : IProjectMembershipRepostiory
    {
        public IEnumerable<ProjectMembership> GetAllProjectMemberships(
            Expression<Func<ProjectMembership, bool>> predicate = null)
        {
            /*
            var session = _sessionProvider.GetCurrentSession();
            return predicate == null
                ? session.Query<ProjectMembership>().ToList()
                : session.Query<ProjectMembership>().Where(predicate).ToList();*/
            return null;
        }
    }
}