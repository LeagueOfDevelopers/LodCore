using System.Net.Mail;
using Common;
using Journalist;
using RabbitMQEventBus;

namespace ContactContext.Events
{
    public class NewContactMessage : EventInfoBase
    {
        public NewContactMessage(
            string clientName, 
            MailAddress clientEmailAddress, 
            string messageTopic,
            string messageBody)
        {
            Require.NotEmpty(clientName, nameof(clientName));
            Require.NotNull(clientEmailAddress, nameof(clientEmailAddress));
            Require.NotEmpty(messageTopic, nameof(messageTopic));
            Require.NotNull(messageBody, nameof(messageBody));

            ClientName = clientName;
            ClientEmailAddress = clientEmailAddress;
            MessageTopic = messageTopic;
            MessageBody = messageBody;
        }

        public string ClientName { get; private set; }

        public MailAddress ClientEmailAddress { get; private set; }

        public string MessageTopic { get; private set; }

        public string MessageBody { get; private set; }
    }
}