using System.Linq.Expressions;
using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class UsersRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public UsersRepository(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<User> AddAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return null;
            }
            return user;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var userToDelete = await _userManager.FindByIdAsync(id.ToString());
            if (userToDelete != null)
            {
                var result = await _userManager.DeleteAsync(userToDelete);
                if (!result.Succeeded)
                {
                    throw new Exception("Cannot find user");
                }
            }
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }


        public async Task<IReadOnlyList<User>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Users.AsQueryable();
            return await query.ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IReadOnlyList<User>> ListAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Users.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            return user;
        }
    }
}