using FinancialControl.Domain.Entities.Shared;

namespace FinancialControl.Domain.Interfaces.Repositories.Shareds
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(params object[] keyValues);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
