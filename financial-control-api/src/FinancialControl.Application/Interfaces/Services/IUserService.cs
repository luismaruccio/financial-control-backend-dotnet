using FinancialControl.Application.Dtos.User.Requests;

namespace FinancialControl.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserRequest request);
    }
}
