namespace LodCore.Infrastructure.DataAccess.Repositories
{
    public interface IProjectRelativesRepository
    {
        int[] GetAllProjectRelativeIds(int projectId);
    }
}