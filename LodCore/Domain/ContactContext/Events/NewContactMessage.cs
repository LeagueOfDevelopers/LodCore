using System.Net.Mail;
using Journalist;
using NotificationService;

namespace ContactContext.Events
{
    public class NewContactMessage : EventInfoBase
    {
        public NewContactMessage(
            string clientName, 
            MailAddress clientEmalAddress, 
            string messageTopic,
            string messageBody)
        {
            Require.NotEmpty(clientName, nameof(clientName));
            Require.NotNull(clientEmalAddress, nameof(clientEmalAddress));
            Require.NotEmpty(messageTopic, nameof(messageTopic));
            Require.NotNull(messageBody, nameof(messageBody));

            ClientName = clientName;
            ClientEmalAddress = clientEmalAddress;
            MessageTopic = messageTopic;
            MessageBody = messageBody;
        }

        public string ClientName { get; private set; }

        public MailAddress ClientEmalAddress { get; private set; }

        public string MessageTopic { get; private set; }

        public string MessageBody { get; private set; }
    }
}