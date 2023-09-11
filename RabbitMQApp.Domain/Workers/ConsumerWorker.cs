using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMQApp.Domain.Settings;
using System.Text;
using RabbitMQApp.Domain.LogMessages;
using Newtonsoft.Json;
using RabbitMQApp.Domain.Entities.Payloads;
using RabbitMQApp.Domain.Entities;
using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQApp.Domain.Tracings;
using OpenQA.Selenium;

namespace RabbitMQApp.Domain.Workers
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ILogger<ConsumerWorker> _logger;

        public ConsumerWorker(IOptions<RabbitMQSettings> options,
            ILogger<ConsumerWorker> logger)
        {
            _logger = logger;
            _rabbitMQSettings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMQSettings.HostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _rabbitMQSettings.QueueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ConsumerReceived;
            channel.BasicConsume(queue: _rabbitMQSettings.QueueName,
                autoAck: true,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(LogMessage.WorkerActive);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        private void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            var json = Encoding.UTF8.GetString(e.Body.ToArray());
            var payloads = JsonConvert.DeserializeObject<List<MessagePayload>>(json)!;
            var eventFile = EventFile.Build(payloads);

            var activityName = $"Arquivos recebidos: {eventFile.Count}";

            using var activity = OpenTelemetryExtensions
                .CreateActivitySource()
                .StartActivity(activityName, ActivityKind.Consumer);

            activity?.SetTag("message", JsonConvert.SerializeObject(eventFile));
            activity?.SetTag("messaging.system", "rabbitmq");
            activity?.SetTag("messaging.destination_kind", "queue");
            activity?.SetTag("messaging.rabbitmq.routing_key", _rabbitMQSettings.QueueName);
            _logger.LogInformation(activityName);
        }
    }
}
