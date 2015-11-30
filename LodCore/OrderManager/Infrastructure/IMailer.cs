using OrderManagement.Domain;

namespace OrderManagement.Infrastructure
{
    public interface IMailer
    {
        void SendNewOrderEmail(Order order);
    }
}