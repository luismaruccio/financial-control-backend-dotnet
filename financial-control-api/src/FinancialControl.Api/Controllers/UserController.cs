using FinancialControl.Application.Dtos.User.Requests;
using FinancialControl.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinancialControl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService, ILogger<UserController> logger, IValidator<CreateUserRequest> createUserValidator) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly IValidator<CreateUserRequest> _createUserValidator = createUserValidator;

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("Request Received - CreateUserRequest: {Request}", request);

            var validationResult = await _createUserValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateUserRequest: {Request} - Errors: {Errors}", request, validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _userService.CreateUserAsync(request);
                stopwatch.Stop();
                _logger.LogInformation("User successfully created: {Request} - Execution time: {ElapsedMilliseconds}ms", request, stopwatch.ElapsedMilliseconds);
                return Ok();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "An error occurred while creating user: {Request} - Execution time: {ElapsedMilliseconds}ms", request, stopwatch.ElapsedMilliseconds);
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
