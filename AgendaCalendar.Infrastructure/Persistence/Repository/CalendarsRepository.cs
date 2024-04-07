using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class CalendarRepository : IRepository<Calendar>
    {
        private readonly AppDbContext _dbContext;

        public CalendarRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Calendar> AddAsync(Calendar calendar, CancellationToken cancellationToken = default)
        {
            var new_calendar = await _dbContext.Calendars.AddAsync(calendar);
            return new_calendar.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var calendarToDelete = _dbContext.Calendars.First(x => x.Id.Equals(id));
            _dbContext.Calendars.Remove(calendarToDelete);
        }

        public async Task<Calendar> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Calendars.AsQueryable();
            query = query.Include(c => c.Events);
            query = query.Include(c => c.Reminders);
            return await query.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<IReadOnlyList<Calendar>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Calendars.AsQueryable();
            query = query.Include(c => c.Events);
            query = query.Include(c => c.Reminders);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<Calendar>> ListAsync(Expression<Func<Calendar, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Calendars.AsQueryable();

            query = query.Include(c => c.Events);
            query = query.Include(c => c.Reminders);

            if (filter != null) query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task<Calendar> UpdateAsync(Calendar calendar, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(calendar).State = EntityState.Modified;
            return calendar;
        }
    }
}