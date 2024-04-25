using System.Linq.Expressions;
using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class EventsRepository : IRepository<Event>
    {
        private readonly AppDbContext _dbContext;

        public EventsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Event> AddAsync(Event ev, CancellationToken cancellationToken = default)
        {
            var @event = await _dbContext.Events.AddAsync(ev);
            return @event.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var eventToDelete = _dbContext.Events.First(x => x.Id.Equals(id));
            _dbContext.Events.Remove(eventToDelete);
        }

        public async Task<Event> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Events.AsQueryable();
            return await query.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<IReadOnlyList<Event>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Events.ToListAsync();
        }

        public async Task<IReadOnlyList<Event>> ListAsync(Expression<Func<Event, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Events.AsQueryable();
            if (filter != null) query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task<Event> UpdateAsync(Event ev, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(ev).State = EntityState.Modified;
            return ev;
        }
    }
}