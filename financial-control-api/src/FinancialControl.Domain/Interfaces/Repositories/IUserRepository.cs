using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories.Shareds;

namespace FinancialControl.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
    }
}
