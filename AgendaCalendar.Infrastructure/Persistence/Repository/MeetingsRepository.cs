using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class MeetingsRepository : IRepository<Meeting>
    {
        private readonly AppDbContext _dbContext;
        public MeetingsRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext; 
        }

        public async Task<Meeting> AddAsync(Meeting entity, CancellationToken cancellationToken = default)
        {
            var meeting = await _dbContext.Meetings.AddAsync(entity);
            return meeting.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var meeting_to_delete = await _dbContext.Meetings.FirstAsync(m => m.Id == id);
            _dbContext.Entry<Meeting>(meeting_to_delete).State = EntityState.Deleted;
        }

        public async Task<Meeting> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Meetings.FirstAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<Meeting>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Meetings.ToListAsync();
        }

        public async Task<IReadOnlyList<Meeting>> ListAsync(Expression<Func<Meeting, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Meetings.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Meeting> UpdateAsync(Meeting entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry<Meeting>(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
