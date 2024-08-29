using FinancialControl.Infra.Bus.Dtos.Notifications;
using FinancialControl.Infra.Bus.Interfaces.Management;
using FinancialControl.Infra.Bus.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace FinancialControl.Infra.Bus.Services
{
    public class NotificationQueueService(IRabbitMQManager rabbitMQManager, ILogger<NotificationQueueService> logger) : INotificationQueueService
    {
        private readonly ILogger<NotificationQueueService> _logger = logger;
        private readonly IRabbitMQManager _rabbitMQManager = rabbitMQManager;

        public void EnqueueSendingEmail(EmailParametersDTO emailParameters)
        {
            var queueName = "notification_queue";
            _rabbitMQManager.EnsureQueueExists(queueName);

            using var channel = _rabbitMQManager.CreateModel();
            var message = JsonSerializer.Serialize(emailParameters);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 mandatory: true,
                                 basicProperties: properties,
                                 body: body);
            _logger.LogInformation("Enqueued email notification for {Recipient} with purpose {Purpose}", emailParameters.UserEmail, emailParameters.EmailPurpose);

        }
    }
}
