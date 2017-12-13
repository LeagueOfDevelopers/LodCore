namespace NotificationService
{
    public interface IUsersRepository
    {
        int[] GetAllUsersIds();

        int[] GetAllAdminIds();
    }
}