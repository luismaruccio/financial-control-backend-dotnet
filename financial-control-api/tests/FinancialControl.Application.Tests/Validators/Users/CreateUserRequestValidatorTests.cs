using FinancialControl.Application.Dtos.User.Requests;
using FinancialControl.Application.Validators.Users;
using FluentValidation.TestHelper;

namespace FinancialControl.Application.Tests.Validators.Users
{
    public class CreateUserRequestValidatorTests
    {
        private readonly CreateUserRequestValidator _validator;

        public CreateUserRequestValidatorTests()
        {
            _validator = new CreateUserRequestValidator();
        }

        [Fact]
        public void Validate_ShouldHaveErrorWhenNameIsEmpty()
        {
            var createUser = new CreateUserRequest("", "test@test.com", "Password");
            var result = _validator.TestValidate(createUser);
            result.ShouldHaveValidationErrorFor(request => request.Name);

        }

        [Fact]
        public void Validate_ShouldHaveErrorWhenEmailIsEmpty()
        {
            var createUser = new CreateUserRequest("Test", "", "Password");
            var result = _validator.TestValidate(createUser);
            result.ShouldHaveValidationErrorFor(request => request.Email);
        }

        [Fact]
        public void Validate_ShouldHaveErrorWhenPasswordIsEmpty()
        {
            var createUser = new CreateUserRequest("Test", "test@test.com", "");
            var result = _validator.TestValidate(createUser);
            result.ShouldHaveValidationErrorFor(request => request.Password);
        }

        [Fact]
        public void Validate_ShouldHaveErrorWhenPasswordIsSmall()
        {
            var createUser = new CreateUserRequest("Test", "test@test.com", "Passw");
            var result = _validator.TestValidate(createUser);
            result.ShouldHaveValidationErrorFor(request => request.Password);
        }
    }
}
