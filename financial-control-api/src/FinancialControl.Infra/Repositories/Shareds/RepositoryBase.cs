using FinancialControl.Domain.Entities.Shared;
using FinancialControl.Domain.Interfaces.Repositories.Shareds;
using FinancialControl.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infra.Repositories.Shareds
{
    public abstract class RepositoryBase<T>(FinancialControlContext context) : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly FinancialControlContext _context = context;

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(params object[] keyValues)
        {
            return await _context.Set<T>().FindAsync(keyValues);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
