using ProjectManagement.Application;
using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IVersionControlSystemGateway
    {
        int CreateRepositoryForProject(CreateProjectRequest request);

        void AddUserToRepository(Project project, int gitlabUserId);

        void RemoveUserFromProject(Project project, int userId);
    }
}