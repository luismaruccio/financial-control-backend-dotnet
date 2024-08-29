using AutoFixture;
using FinancialControl.Application.Services;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infra.Bus.Dtos.Notifications;
using FinancialControl.Infra.Bus.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.RegularExpressions;

namespace FinancialControl.Application.Tests.Services
{
    public partial class ValidationEmailServiceTest
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IValidationCodeRepository> _validationCodeRepositoryMock;
        private readonly Mock<INotificationQueueService> _notificationQueueServiceMock;
        private readonly Mock<ILogger<ValidationEmailService>> _loggerMock;
        private readonly ValidationEmailService _validationEmailService;

        public ValidationEmailServiceTest()
        {
            _validationCodeRepositoryMock = new Mock<IValidationCodeRepository>();
            _notificationQueueServiceMock = new Mock<INotificationQueueService>();
            _loggerMock = new Mock<ILogger<ValidationEmailService>>();
            _validationEmailService = new ValidationEmailService(_validationCodeRepositoryMock.Object,
                                                                 _notificationQueueServiceMock.Object,
                                                                 _loggerMock.Object);
        }

        [Fact]
        public async Task SendValidationEmailAsync_ShouldGenerateAValidCode()
        {
            var user = _fixture.Create<User>();
            await _validationEmailService.SendValidationEmailAsync(user);

            _validationCodeRepositoryMock.Verify(
                mock => mock.AddAsync(It.Is<ValidationCode>(vc =>
                    ValidationCodeRegex().IsMatch(vc.Code)
                ))
            );
        }

        [Fact]
        public async Task SendValidationEmailAsync_ShouldRemoveTheOldCode()
        {
            var user = _fixture.Create<User>();
            var oldValidationCode = _fixture.Create<ValidationCode>();

            _validationCodeRepositoryMock.Setup(
                mock => 
                    mock.GetByPurpouseAndUserId(ValidationCodePurpose.EmailValidation, user.Id)
                ).ReturnsAsync(oldValidationCode);

            await _validationEmailService.SendValidationEmailAsync(user);

            _validationCodeRepositoryMock.Verify(mock => mock.DeleteAsync(oldValidationCode));
        }

        [Fact]
        public async Task SendValidationEmailAsync_ShouldCreateAValidationCode()
        {
            var user = _fixture.Create<User>();
            await _validationEmailService.SendValidationEmailAsync(user);

            _validationCodeRepositoryMock.Verify(
                mock => mock.AddAsync(It.IsAny<ValidationCode>())
            );
        }

        [Fact]
        public async Task SendValidationEmailAsync_ShouldEnqueueSendingEmail()
        {
            var user = _fixture.Create<User>();
            await _validationEmailService.SendValidationEmailAsync(user);

            _notificationQueueServiceMock.Verify(
                mock => mock.EnqueueSendingEmail(It.IsAny<EmailParametersDTO>())
            );
        }

        [GeneratedRegex(@"^\d{4}-\d{4}$")]
        private static partial Regex ValidationCodeRegex();
    }
}
