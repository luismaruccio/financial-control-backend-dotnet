using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces.Services
{
    public interface IValidationEmailService
    {
        Task SendValidationEmailAsync(User user);
    }
}
