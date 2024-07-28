using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infra.Data;
using FinancialControl.Infra.Repositories.Shareds;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infra.Repositories
{
    public class ValidationCodeRepository(FinancialControlContext context) : RepositoryBase<ValidationCode>(context), IValidationCodeRepository
    {
        public async Task<ValidationCode?> GetByIdAsync(int id, int userId) => 
            await GetByIdAsync([id, userId]);

        public async Task<ValidationCode?> GetByCodeAndUserId(string code, int userId)
        {
            return await _context.Set<ValidationCode>().FirstOrDefaultAsync(vc => vc.Code == code && vc.UserId == userId);
        }
    }
}
