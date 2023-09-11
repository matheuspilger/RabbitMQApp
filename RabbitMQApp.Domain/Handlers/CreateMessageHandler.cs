using MediatR;
using RabbitMQApp.Domain.Commands;
using RabbitMQApp.Domain.Entities.Payloads;
using RabbitMQApp.Domain.Services.Interfaces;

namespace RabbitMQApp.Domain.Handlers
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageRequest>
    {
        private readonly ICreateMessageService _createMessageService;

        public CreateMessageHandler(ICreateMessageService createMessageService)
        {
            _createMessageService = createMessageService;
        }

        public async Task Handle(CreateMessageRequest request, CancellationToken cancellationToken)
        {
            var payload = MessagePayload.Build(request.FileNames);
            await Task.Run(() => _createMessageService.Publish(payload), cancellationToken);
        }
    }
}