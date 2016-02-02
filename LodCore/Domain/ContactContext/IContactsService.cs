using ContactContext.Events;

namespace ContactContext
{
    public interface IContactsService
    {
        void SendContactMessage(NewContactMessage contactMessage);
    }
}