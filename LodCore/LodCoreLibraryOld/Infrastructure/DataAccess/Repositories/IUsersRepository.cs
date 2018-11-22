namespace LodCoreLibraryOld.Infrastructure.DataAccess.Repositories
{
    public interface IUsersRepository
    {
        int[] GetAllUsersIds();

        int[] GetAllAdminIds();
    }
}