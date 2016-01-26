namespace ProjectManagement.Application
{
    public interface IUserRoleAnalyzer
    {
        string GetUserCommonRole(int userId);
    }
}