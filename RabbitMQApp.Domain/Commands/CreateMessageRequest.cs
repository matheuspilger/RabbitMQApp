using MediatR;

namespace RabbitMQApp.Domain.Commands
{
    public class CreateMessageRequest: IRequest
    {
        public List<string> FileNames { get; set; }
    }
}
