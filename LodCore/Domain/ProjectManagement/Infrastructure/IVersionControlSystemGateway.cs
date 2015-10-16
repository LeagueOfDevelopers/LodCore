using System;
using ProjectManagement.Application;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IVersionControlSystemGateway
    {
        Uri CreateRepositoryForProject(CreateProjectRequest request);

        void AddUserToProject(Project project, int userId);

        void RemoveUserFromProject(Project project, int userId);
    }
}