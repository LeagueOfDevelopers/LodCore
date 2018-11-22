using LodCoreLibraryOld.Domain.ProjectManagment;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface IProjectRepository
    {
        Project[] GetAllProjects(Func<Project, bool> criteria = null);

        Project[] GetSomeProjects(int skipCount, int takeCount, Expression<Func<Project, int>> orderer = null, Expression<Func<Project, bool>> predicate = null);

        Project GetProject(int projectId);

        int SaveProject(Project project);

        void UpdateProject(Project project);

        IEnumerable<string> GetUserRoles(int userId);
    }
}