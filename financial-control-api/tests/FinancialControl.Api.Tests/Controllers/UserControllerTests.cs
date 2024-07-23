using AutoFixture;
using FinancialControl.Api.Controllers;
using FinancialControl.Application.Dtos.User.Requests;
using FinancialControl.Application.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinancialControl.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly Mock<IValidator<CreateUserRequest>> _createUserValidatorMock;
        private readonly UserController _controller;
        private readonly Fixture _fixture;

        public UserControllerTests() 
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _createUserValidatorMock = new Mock<IValidator<CreateUserRequest>>();
            _controller = new(_userServiceMock.Object, _loggerMock.Object, _createUserValidatorMock.Object); 
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenValidationFails()
        {
            var request = _fixture.Create<CreateUserRequest>();
            var validationFailures = new List<ValidationFailure> { new("Username", "Username is required") };
            var validationResult = new ValidationResult(validationFailures);

            _createUserValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);

            var result = await _controller.CreateUser(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task CreateUser_ShouldCallCreateUserAsync_WhenValidationPasses()
        {
            var request = _fixture.Create<CreateUserRequest>();
            var validationResult = new ValidationResult();

            _createUserValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);

            var result = await _controller.CreateUser(request);

            _userServiceMock.Verify(s => s.CreateUserAsync(request), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            var request = _fixture.Create<CreateUserRequest>();
            var validationResult = new ValidationResult();

            _createUserValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);

            _userServiceMock.Setup(s => s.CreateUserAsync(request))
                .ThrowsAsync(new Exception("Some error"));

            var result = await _controller.CreateUser(request);

            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("An internal server error occurred.", internalServerErrorResult.Value);
        }

    }
}
