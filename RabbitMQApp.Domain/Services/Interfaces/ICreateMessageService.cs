using RabbitMQApp.Domain.Entities.Payloads;

namespace RabbitMQApp.Domain.Services.Interfaces
{
    public interface ICreateMessageService
    {
        void Publish(IEnumerable<MessagePayload> payloads);
    }
}
