using FinancialControl.Application.Services;

namespace FinancialControl.Application.Tests.Services
{
    public class EncryptionServiceTest
    {
        private readonly EncryptionService _encryptionService = new();

        [Fact]
        public void HashPassword_ShouldReturnAHashedPassword()
        {
            var hashedPassword = _encryptionService.HashPassword("somepassword");
            
            Assert.NotEmpty(hashedPassword);
        }

        [Fact]
        public void VerifyPassword_ShouldValidateAValidPasswordAsExpected()
        {
            var encryptionService = new EncryptionService();
            var validPassword = "ValidPassword123";
            var validHash = encryptionService.HashPassword(validPassword);

            var result = _encryptionService.VerifyPassword(validPassword, validHash);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldValidateAInvalidPasswordAsExpected()
        {
            var encryptionService = new EncryptionService();
            var validHash = encryptionService.HashPassword("ValidPassword123");
            var invalidPassword = "InvalidPassword123";

            var result = _encryptionService.VerifyPassword(invalidPassword, validHash);

            Assert.False(result);
        }

    }
}
