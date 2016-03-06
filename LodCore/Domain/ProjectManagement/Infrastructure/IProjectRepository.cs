using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IProjectRepository
    {
        Project[] GetAllProjects(Func<Project, bool> criteria = null);

        Project[] GetSomeProjects(int skipCount, int takeCount, Expression<Func<Project, bool>> predicate = null);
        
        Project GetProject(int projectId);

        int SaveProject(Project project);

        void UpdateProject(Project project);

        IEnumerable<string> GetUserRoles(int userId);
    }
}