using ProjectManagement.Domain;

namespace ProjectManagement.Infrastructure
{
    public interface IProjectRepository
    {
        Project GetProject(int projectId);
    }
}