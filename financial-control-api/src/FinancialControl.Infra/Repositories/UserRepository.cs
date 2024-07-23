using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infra.Data;
using FinancialControl.Infra.Repositories.Shareds;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infra.Repositories
{
    public class UserRepository(FinancialControlContext context) : RepositoryBase<User>(context), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
