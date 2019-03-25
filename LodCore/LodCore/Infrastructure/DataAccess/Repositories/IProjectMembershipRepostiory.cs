using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCore.Domain.ProjectManagment;

namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public interface IProjectMembershipRepostiory
    {
        IEnumerable<ProjectMembership> GetAllProjectMemberships(
            Expression<Func<ProjectMembership, bool>> predicate = null);
    }
}