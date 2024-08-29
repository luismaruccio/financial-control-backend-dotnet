using RabbitMQ.Client;

namespace FinancialControl.Infra.Bus.Interfaces.Connections
{
    public interface IRabbitMQConnection
    {
        IModel CreateModel();
        bool IsConnected { get; }
        void TryConnect();
    }
}
