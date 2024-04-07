using System.Linq.Expressions;

namespace AgendaCalendar.Domain.Abstractions
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetListAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}