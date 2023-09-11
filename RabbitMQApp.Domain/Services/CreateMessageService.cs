using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQApp.Domain.Entities.Payloads;
using RabbitMQApp.Domain.LogMessages;
using RabbitMQApp.Domain.Services.Interfaces;
using RabbitMQApp.Domain.Settings;
using System.Text;

namespace RabbitMQApp.Domain.Services
{
    public class CreateMessageService : ICreateMessageService
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ILogger<CreateMessageService> _logger;

        public CreateMessageService(IOptions<RabbitMQSettings> options,
            ILogger<CreateMessageService> logger)
        {
            _rabbitMQSettings = options.Value;
            _logger = logger;
        }

        public void Publish(IEnumerable<MessagePayload> payloads)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _rabbitMQSettings.HostName };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _rabbitMQSettings.QueueName, 
                    durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(payloads);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "", routingKey: _rabbitMQSettings.QueueName, body: body);

                _logger.LogInformation(LogMessage.SuccessPublished, payloads);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, message: LogMessage.ErrorPublishing, payloads);
            }
        }
    }
}
