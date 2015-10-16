using System;
using ProjectManagement.Application;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IProjectManagerGateway
    {
        void AddNewUserToProject(Project project, int userId);
        
        void RemoveUserFromProject(Project project, int userId);

        Uri CreateProject(CreateProjectRequest request);
    }
}
