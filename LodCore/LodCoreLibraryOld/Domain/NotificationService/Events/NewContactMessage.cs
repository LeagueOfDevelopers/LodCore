using Journalist;

namespace LodCoreLibraryOld.Domain.NotificationService
{
    public class NewContactMessage : EventInfoBase
    {
        public NewContactMessage(
            string clientName,
            string clientEmailAddress,
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

        public string ClientName { get; }

        public string ClientEmailAddress { get; }

        public string MessageTopic { get; }

        public string MessageBody { get; }
    }
}