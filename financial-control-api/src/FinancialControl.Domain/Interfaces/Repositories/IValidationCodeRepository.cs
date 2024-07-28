using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories.Shareds;

namespace FinancialControl.Domain.Interfaces.Repositories
{
    public interface IValidationCodeRepository : IRepositoryBase<ValidationCode>
    {
        Task<ValidationCode?> GetByIdAsync(int id, int userId);
        Task<ValidationCode?> GetByCodeAndUserId(string code, int userId);
    }
}
