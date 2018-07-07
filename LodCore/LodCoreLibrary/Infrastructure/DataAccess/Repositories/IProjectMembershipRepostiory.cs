using LodCoreLibrary.Domain.ProjectManagment;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LodCoreLibrary.Infrastructure.DataAccess.Repositories
{
    public interface IProjectMembershipRepostiory
    {
        IEnumerable<ProjectMembership> GetAllProjectMemberships(Expression<Func<ProjectMembership, bool>> predicate = null);
    }
}