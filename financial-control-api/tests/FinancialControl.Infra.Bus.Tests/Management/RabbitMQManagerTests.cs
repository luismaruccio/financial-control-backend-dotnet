using AutoFixture;
using FinancialControl.Infra.Bus.Interfaces.Connections;
using FinancialControl.Infra.Bus.Management;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;

namespace FinancialControl.Infra.Bus.Tests.Management
{
    public class RabbitMQManagerTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IRabbitMQConnection> _rabbitMQConnectionMock;
        private readonly Mock<ILogger<RabbitMQManager>> _loggerMock;
        private readonly RabbitMQManager _rabbitMQManager;

        public RabbitMQManagerTests()
        {
            _rabbitMQConnectionMock = new Mock<IRabbitMQConnection>();
            _loggerMock = new Mock<ILogger<RabbitMQManager>>();

            _rabbitMQManager = new RabbitMQManager(_rabbitMQConnectionMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void CreateModel_ShouldTryToConnect_IfNotConnected()
        {
            _rabbitMQConnectionMock.Setup(conn => conn.IsConnected).Returns(false);

            var model = _rabbitMQManager.CreateModel();

            _rabbitMQConnectionMock.Verify(conn => conn.TryConnect(), Times.Once);
            _rabbitMQConnectionMock.Verify(conn => conn.CreateModel(), Times.Once);
        }

        [Fact]
        public void CreateModel_ShouldNotTryToConnect_IfAlreadyConnected()
        {
            _rabbitMQConnectionMock.Setup(conn => conn.IsConnected).Returns(true);

            var model = _rabbitMQManager.CreateModel();

            _rabbitMQConnectionMock.Verify(conn => conn.TryConnect(), Times.Never);
            _rabbitMQConnectionMock.Verify(conn => conn.CreateModel(), Times.Once);
        }

        [Fact]
        public void EnsureQueueExists_ShouldDeclareQueue()
        {
            var queueName = "testQueue";
            var modelMock = new Mock<IModel>();

            _rabbitMQConnectionMock.Setup(conn => conn.CreateModel()).Returns(modelMock.Object);

            _rabbitMQManager.EnsureQueueExists(queueName);

            modelMock.Verify(model => model.QueueDeclare(queueName, true, false, false, null), Times.Once);
        }
    }
}
