namespace LodCore.Infrastructure.Mailing.EmailModels
{
    public class AdminNotification
    {
        public AdminNotification(string userName, string message)
        {
            UserName = userName;
            Message = message;
        }

        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
