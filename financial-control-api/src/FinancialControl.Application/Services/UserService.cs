using FinancialControl.Application.Dtos.User.Requests;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FinancialControl.Application.Services
{
    public class UserService(IUserRepository userRepository, IEncryptionService encryptionService, IValidationEmailService validationEmailService, ILogger<UserService> logger) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IValidationEmailService _validationEmailService = validationEmailService;
        private readonly ILogger<UserService> _logger = logger;

        public async Task CreateUserAsync(CreateUserRequest request)
        {
            _logger.LogInformation("Creating user with email: {Email}", request.Email);

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Email {Email} is already in use.", request.Email);
                throw new InvalidOperationException("Email is already in use.");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _encryptionService.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);
            _logger.LogInformation("User created successfully with email: {Email}", request.Email);

            await _validationEmailService.SendValidationEmailAsync(user);

        }
    }
}
