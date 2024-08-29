using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infra.Bus.Enums;
using FinancialControl.Infra.Bus.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FinancialControl.Application.Services
{
    public class ValidationEmailService(IValidationCodeRepository validationCodeRepository, INotificationQueueService notificationService, ILogger<ValidationEmailService> logger) : IValidationEmailService
    {
        private readonly IValidationCodeRepository _validationCodeRepository = validationCodeRepository;
        private readonly INotificationQueueService _notificationQueueService = notificationService;
        private readonly ILogger<ValidationEmailService> _logger = logger;

        public async Task SendValidationEmailAsync(User user)
        {
            var validationCode = await GenerateAValidValidationCode(user.Id);

            await RemoveTheOldValidationCode(ValidationCodePurpose.EmailValidation, user.Id);

            var validationCodeEntity = new ValidationCode
            {
                Code = validationCode,
                UserId = user.Id,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            await _validationCodeRepository.AddAsync(validationCodeEntity);

            _notificationQueueService.EnqueueSendingEmail(new(
                UserName: user.Name,
                UserEmail: user.Email,
                Code: validationCode,
                EmailPurpose: EmailPurpose.EmailVerification
            ));

            _logger.LogInformation("Validation email sent to {Email}", user.Email);
        }

        private async Task<string> GenerateAValidValidationCode(int userId)
        {
            string validationCode = "";
            var codeInUse = true;

            while (codeInUse)
            {
                validationCode = GenerateValidationCode();
                codeInUse = await _validationCodeRepository.GetByCodeAndUserId(validationCode, userId) != null;
            }

            return validationCode;
        }

        private async Task RemoveTheOldValidationCode(ValidationCodePurpose purpose, int userId)
        {
            var oldValidationCode = await _validationCodeRepository.GetByPurpouseAndUserId(purpose, userId);

            if (oldValidationCode != null)
            {
                await _validationCodeRepository.DeleteAsync(oldValidationCode);
                _logger.LogInformation("Removed the old validation code {Code} for the userID {UserID}", [oldValidationCode.Code, oldValidationCode.UserId]);
            }
        }

        private static string GenerateValidationCode()
        {
            var random = new Random();
            var part1 = random.Next(1000, 9999).ToString("D4");
            var part2 = random.Next(1000, 9999).ToString("D4");
            return $"{part1}-{part2}";
        }
    }
}
