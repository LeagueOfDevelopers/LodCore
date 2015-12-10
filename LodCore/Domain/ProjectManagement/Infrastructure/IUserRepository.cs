namespace ProjectManagement.Infrastructure
{
    public interface IUserRepository
    {
        int GetUserRedmineId(int userId);

        int GetUserGitlabId(int userId);
    }
}