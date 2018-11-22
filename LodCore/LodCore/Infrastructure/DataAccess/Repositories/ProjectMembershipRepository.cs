using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Journalist;
using LodCore.Common;
using LodCore.Domain.ProjectManagment;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public class ProjectMembershipRepository : IProjectMembershipRepostiory
    {
        public ProjectMembershipRepository()
        {
        }


        public IEnumerable<ProjectMembership> GetAllProjectMemberships(Expression<Func<ProjectMembership, bool>> predicate = null)
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