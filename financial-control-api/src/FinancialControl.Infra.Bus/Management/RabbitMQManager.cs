using FinancialControl.Infra.Bus.Interfaces.Connections;
using FinancialControl.Infra.Bus.Interfaces.Management;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FinancialControl.Infra.Bus.Management
{
    public class RabbitMQManager(IRabbitMQConnection rabbitMQConnection, ILogger<RabbitMQManager> logger) : IRabbitMQManager
    {
        private readonly IRabbitMQConnection _rabbitMQConnection = rabbitMQConnection;
        private readonly ILogger<RabbitMQManager> _logger = logger;

        public IModel CreateModel()
        {
            if (!_rabbitMQConnection.IsConnected)
            {
                _rabbitMQConnection.TryConnect();
            }

            return _rabbitMQConnection.CreateModel();
        }

        public void EnsureQueueExists(string queueName)
        {
            using var channel = CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _logger.LogInformation("Queue declared: {QueueName}", queueName);
        }
    }
}
