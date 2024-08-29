using FinancialControl.Infra.Bus.Connections;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialControl.Infra.Bus.Tests.Connections
{
    public class RabbitMQConnectionTests
    {
        private readonly Mock<IConnectionFactory> _connectionFactoryMock;
        private readonly Mock<IConnection> _connectionMock;
        private readonly Mock<ILogger<RabbitMQConnection>> _loggerMock;
        private readonly RabbitMQConnection _rabbitMQConnection;

        public RabbitMQConnectionTests()
        {
            _connectionFactoryMock = new Mock<IConnectionFactory>();
            _connectionMock = new Mock<IConnection>();
            _loggerMock = new Mock<ILogger<RabbitMQConnection>>();

            _rabbitMQConnection = new RabbitMQConnection(_connectionFactoryMock.Object, _loggerMock.Object);

            _connectionFactoryMock.Setup(f => f.CreateConnection()).Returns(_connectionMock.Object);

            var endpointMock = new Mock<AmqpTcpEndpoint>();

            _connectionMock.Setup(c => c.Endpoint).Returns(endpointMock.Object);
        }

        [Fact]
        public void IsConnected_ShouldReturnFalse_WhenNotConnected()
        {
            var isConnected = _rabbitMQConnection.IsConnected;

            Assert.False(isConnected);
        }

        [Fact]
        public void IsConnected_ShouldReturnTrue_WhenConnected()
        {
            _connectionMock.Setup(c => c.IsOpen).Returns(true);
            _rabbitMQConnection.TryConnect();

            var isConnected = _rabbitMQConnection.IsConnected;

            Assert.True(isConnected);
        }

        [Fact]
        public void CreateModel_ShouldThrowException_WhenNotConnected()
        {
            Assert.Throws<InvalidOperationException>(() => _rabbitMQConnection.CreateModel());
        }

        [Fact]
        public void CreateModel_ShouldReturnModel_WhenConnected()
        {
            var modelMock = new Mock<IModel>();
            _connectionMock.Setup(c => c.IsOpen).Returns(true);
            _connectionMock.Setup(c => c.CreateModel()).Returns(modelMock.Object);

            _rabbitMQConnection.TryConnect();

            var model = _rabbitMQConnection.CreateModel();

            Assert.NotNull(model);
            Assert.Equal(modelMock.Object, model);
        }


        [Fact]
        public void EnsureEventsAreSubscribed_WhenConnected()
        {
            _connectionMock.Setup(c => c.IsOpen).Returns(true);

            _rabbitMQConnection.TryConnect();

            _connectionMock.VerifyAdd(c => c.ConnectionShutdown += It.IsAny<EventHandler<ShutdownEventArgs>>(), Times.Once);
            _connectionMock.VerifyAdd(c => c.CallbackException += It.IsAny<EventHandler<CallbackExceptionEventArgs>>(), Times.Once);
            _connectionMock.VerifyAdd(c => c.ConnectionBlocked += It.IsAny<EventHandler<ConnectionBlockedEventArgs>>(), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldDisposeConnection_WhenCalled()
        {
            _connectionMock.Setup(c => c.IsOpen).Returns(true);
            _rabbitMQConnection.TryConnect();

            _rabbitMQConnection.Dispose();

            _connectionMock.Verify(c => c.Dispose(), Times.Once);
        }
    }
}
