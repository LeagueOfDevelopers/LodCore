using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IProjectMembershipRepostiory
    {
        IEnumerable<ProjectMembership> GetAllProjectMemberships(Expression<Func<ProjectMembership, bool>> predicate = null);
    }
}