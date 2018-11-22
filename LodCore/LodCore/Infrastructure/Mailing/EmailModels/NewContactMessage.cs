namespace LodCore.Infrastructure.Mailing.EmailModels
{
    public class NewContactMessage
    {
        public NewContactMessage(
            string userName,
            string senderName,
            string topic,
            string message,
            string senderEmail)
        {
            UserName = userName;
            SenderName = senderName;
            Topic = topic;
            Message = message;
            SenderEmail = senderEmail;
        }

        public string UserName { get; set; }
        public string SenderName { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public string SenderEmail { get; set; }
    }
}
