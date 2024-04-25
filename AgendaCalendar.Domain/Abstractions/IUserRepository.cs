using AgendaCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace AgendaCalendar.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user, string password, CancellationToken cancellationToken = default);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> GetListAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> ListAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default);
        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
