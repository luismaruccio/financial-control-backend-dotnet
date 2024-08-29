using AutoFixture;
using FinancialControl.Infra.Bus.Dtos.Notifications;
using FinancialControl.Infra.Bus.Interfaces.Management;
using FinancialControl.Infra.Bus.Services;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FinancialControl.Infra.Bus.Tests.Services
{
    public class NotificationQueueServiceTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IRabbitMQManager> _rabbitMQManagerMock;
        private readonly Mock<ILogger<NotificationQueueService>> _logger;
        private readonly Mock<IModel> _channelMock;
        private readonly Mock<IBasicProperties> _basicPropertiesMock;
        private readonly NotificationQueueService _notificationQueueService;
        private const string _queueName = "notification_queue";

        public NotificationQueueServiceTests()
        {
            _rabbitMQManagerMock = new Mock<IRabbitMQManager>();
            _logger = new Mock<ILogger<NotificationQueueService>>();
            _channelMock = new Mock<IModel>();

            _basicPropertiesMock = new Mock<IBasicProperties>();

            _channelMock.Setup(
                mock => mock.CreateBasicProperties()
            ).Returns(_basicPropertiesMock.Object);

            _rabbitMQManagerMock.Setup(
                mock => mock.CreateModel()
            ).Returns(_channelMock.Object);

            _notificationQueueService = new(_rabbitMQManagerMock.Object, _logger.Object);
        }

        [Fact]
        public void EnqueueSendingEmail_ShouldCreateQueue()
        {
            var emailParameters = _fixture.Create<EmailParametersDTO>();

            _notificationQueueService.EnqueueSendingEmail(emailParameters);

            _rabbitMQManagerMock.Verify(
                mock => mock.EnsureQueueExists(_queueName), Times.Once);
        }

        [Fact]
        public void EnqueueSendingEmail_ShouldPublishMessage()
        {
            var emailParameters = _fixture.Create<EmailParametersDTO>();

            var message = JsonSerializer.Serialize(emailParameters);
            var body = Encoding.UTF8.GetBytes(message);

            _notificationQueueService.EnqueueSendingEmail(emailParameters);

            _channelMock.Verify(
                mock => mock.BasicPublish(
                    "",
                    _queueName,
                    true,
                    It.IsAny<IBasicProperties>(),
                    It.IsAny<ReadOnlyMemory<byte>>()
                ));

        }


    }
}
