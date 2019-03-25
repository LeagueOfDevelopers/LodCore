using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LodCoreLibraryOld.Domain.ProjectManagment;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface IProjectMembershipRepostiory
    {
        IEnumerable<ProjectMembership> GetAllProjectMemberships(
            Expression<Func<ProjectMembership, bool>> predicate = null);
    }
}