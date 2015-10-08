namespace NotificationService
{
    public interface IProjectRelativesRepository
    {
        int[] GetAllProjectRelativeIds(int projectId);
    }
}