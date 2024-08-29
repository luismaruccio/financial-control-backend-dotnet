using RabbitMQ.Client;

namespace FinancialControl.Infra.Bus.Interfaces.Management
{
    public interface IRabbitMQManager
    {
        void EnsureQueueExists(string queueName);
        IModel CreateModel();
    }
}
