using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class SlotRepository : IRepository<Slot>
    {
        private readonly AppDbContext _dbContext;

        public SlotRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Slot> AddAsync(Slot calendar, CancellationToken cancellationToken = default)
        {
            var new_slot = await _dbContext.Slots.AddAsync(calendar);
            return new_slot.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var slotToDelete = _dbContext.Slots.First(x => x.Id.Equals(id));
            _dbContext.Slots.Remove(slotToDelete);
        }

        public async Task<Slot> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Slots.AsQueryable();
            return await query.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<IReadOnlyList<Slot>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Slots.AsQueryable();
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<Slot>> ListAsync(Expression<Func<Slot, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Slots.AsQueryable();

            if (filter != null) query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task<Slot> UpdateAsync(Slot calendar, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(calendar).State = EntityState.Modified;
            return calendar;
        }
    }
}
