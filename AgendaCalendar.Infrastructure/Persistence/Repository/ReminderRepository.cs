using System.Linq.Expressions;
using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class ReminderRepository : IRepository<Reminder>
    {
        private readonly AppDbContext _dbContext;

        public ReminderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Reminder> AddAsync(Reminder reminder, CancellationToken cancellationToken = default)
        {
            var new_remidner = await _dbContext.Reminders.AddAsync(reminder);
            return new_remidner.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var reminderToDelete = _dbContext.Reminders.First(x => x.Id.Equals(id));
            _dbContext.Reminders.Remove(reminderToDelete);
        }

        public async Task<Reminder> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Reminders.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IReadOnlyList<Reminder>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Reminders.ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<Reminder>> ListAsync(Expression<Func<Reminder, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Reminders.AsQueryable();
            if (filter != null) query = query.Where(filter);

            return query.ToList();
        }

        public async Task<Reminder> UpdateAsync(Reminder reminder, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(reminder).State = EntityState.Modified;
            return reminder;
        }
    }
}