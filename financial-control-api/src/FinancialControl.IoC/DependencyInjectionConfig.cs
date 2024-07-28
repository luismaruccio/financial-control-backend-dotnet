using FinancialControl.Application.Dtos.User.Requests;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Application.Services;
using FinancialControl.Application.Validators.Users;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infra.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialControl.IoC
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IUserService, UserService>();

            //Validators
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
            
            // Domain

            // Infra
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IValidationCodeRepository, ValidationCodeRepository>();

            return services;
        }
    }
}
