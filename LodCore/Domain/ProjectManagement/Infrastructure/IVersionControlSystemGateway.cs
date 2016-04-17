using ProjectManagement.Application;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IVersionControlSystemGateway
    {
        VersionControlSystemInfo CreateRepositoryForProject(CreateProjectRequest request);

        VersionControlSystemInfo UpdateRepositoryForProject(Project projectToUpdate);

        void AddUserToRepository(Project project, int gitlabUserId);

        void RemoveUserFromProject(Project project, int userId);
    }
}