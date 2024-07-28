using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infra.Data;
using FinancialControl.Infra.Repositories;
using FinancialControl.Infra.Tests.Repositories.Shared;

namespace FinancialControl.Infra.Tests.Repositories
{
    public class ValidationCodeRepositoryTests : TestBase
    {
        private readonly ValidationCodeRepository _repository;
        private readonly int _userId;

        public ValidationCodeRepositoryTests()
        {
            _repository = new ValidationCodeRepository(Context);
            _userId = PrepareDatabase(Context).Result;
        }

        [Fact]
        public async Task AddAsync_ShouldAddValidationCode()
        {
            var validationCode = new ValidationCode()
            {
                Code = "1234-5678",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            await _repository.AddAsync(validationCode);
            var validationCodes = await _repository.GetAllAsync();

            Assert.Single(validationCodes);
            Assert.Equal("1234-5678", validationCodes.First().Code);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnValidationCode()
        {
            var validationCode = new ValidationCode()
            {
                Code = "1234-5678",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            await _repository.AddAsync(validationCode);
            var retrievedValidationCode = await _repository.GetByIdAsync(validationCode.Id, _userId);

            Assert.NotNull(retrievedValidationCode);
            Assert.Equal("1234-5678", retrievedValidationCode.Code);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllValidationCodes()
        {
            var validationCode1 = new ValidationCode()
            {
                Code = "1234-5678",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            var validationCode2 = new ValidationCode()
            {
                Code = "4321-8765",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            await _repository.AddAsync(validationCode1);
            await _repository.AddAsync(validationCode2);

            var validationCodes = await _repository.GetAllAsync();

            Assert.Equal(2, validationCodes.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateValidationCode()
        {
            var validationCode = new ValidationCode()
            {
                Code = "1234-5678",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            await _repository.AddAsync(validationCode);

            validationCode.Code = "4321-8765";
            await _repository.UpdateAsync(validationCode);
            var updatedValidationCode = await _repository.GetByIdAsync(validationCode.Id, _userId);

            Assert.Equal(updatedValidationCode!.Code, validationCode.Code);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveValidationCode()
        {
            var validationCode = new ValidationCode()
            {
                Code = "1234-5678",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            await _repository.AddAsync(validationCode);

            await _repository.DeleteAsync(validationCode);

            var validationCodes = await _repository.GetAllAsync();

            Assert.Empty(validationCodes);
        }

        [Fact]
        public async Task GetByCodeAndUserId_ShouldGetValidationCodeByCodeAndUserId()
        {
            var validationCode1 = new ValidationCode()
            {
                Code = "1234-5678",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            var validationCode2 = new ValidationCode()
            {
                Code = "4321-8765",
                UserId = _userId,
                Purpose = ValidationCodePurpose.EmailValidation,
                Expiration = DateTime.UtcNow.AddHours(1),
            };

            await _repository.AddAsync(validationCode1);
            await _repository.AddAsync(validationCode2);

            var validationCodeReturned = await _repository.GetByCodeAndUserId("1234-5678", _userId);

            Assert.NotNull(validationCodeReturned);
        }

        private static async Task<int> PrepareDatabase(FinancialControlContext context)
        {
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = Guid.NewGuid().ToString(),
            };

            await context.Set<User>().AddAsync(user);
            await context.SaveChangesAsync();
            return user.Id;
        }
    }
}
