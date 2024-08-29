using AutoFixture;
using FinancialControl.Application.Dtos.User.Requests;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Application.Services;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinancialControl.Application.Tests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEncryptionService> _encryptionServiceMock;
        private readonly Mock<IValidationEmailService> _validationEmailServiceMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _userService;        
        private readonly Fixture _fixture;

        public UserServiceTest()
        {
            _fixture = new Fixture();

            _userRepositoryMock = new Mock<IUserRepository>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _validationEmailServiceMock = new Mock<IValidationEmailService>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepositoryMock.Object, _encryptionServiceMock.Object, _validationEmailServiceMock.Object, _loggerMock.Object);

            _encryptionServiceMock.Setup(m => m.HashPassword(It.IsAny<string>())).Returns("HashedPassword");
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateAUserAsExpected()
        {
            var request = _fixture.Create<CreateUserRequest>();

            await _userService.CreateUserAsync(request);

            _userRepositoryMock.Verify(r =>
                r.AddAsync(It.Is<User>(u =>
                    u.Name == request.Name &&
                    u.Email == request.Email &&
                    u.PasswordHash == "HashedPassword"
                )
            ),
            Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowInvalidOperationException_WhenEmailIsAlreadyInUse()
        {
            var request = _fixture.Create<CreateUserRequest>();

            _userRepositoryMock.Setup(mock => mock.GetByEmailAsync(request.Email)).ReturnsAsync(_fixture.Create<User>());

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateUserAsync(request));
            Assert.Equal("Email is already in use.", exception.Message);
        }
    }
}
