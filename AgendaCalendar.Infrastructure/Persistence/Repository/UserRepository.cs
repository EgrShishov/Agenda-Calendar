using System.Linq.Expressions;
using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class UsersRepository : IRepository<User>
    {
        private readonly AppDbContext _dbContext;

        public UsersRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            var new_user = await _dbContext.Users.AddAsync(user);
            return new_user.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var userToDelete = _dbContext.Users.First(x => x.Id.Equals(id));
            _dbContext.Users.Remove(userToDelete);
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<IReadOnlyList<User>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> ListAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Users.AsQueryable();
            if (filter != null) query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            return user;
        }
    }
}