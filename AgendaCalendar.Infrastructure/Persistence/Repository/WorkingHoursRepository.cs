using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class WorkingHoursRepository : IRepository<WorkingHours>
    {
        private readonly AppDbContext _dbContext;

        public WorkingHoursRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkingHours> AddAsync(WorkingHours entity, CancellationToken cancellationToken = default)
        {
            var working_hours = await _dbContext.AddAsync(entity);
            return working_hours.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var working_hours_to_delete = await _dbContext.WorkingHours.FirstAsync(w => w.Id == id);
            _dbContext.Entry<WorkingHours>(working_hours_to_delete).State = EntityState.Deleted;
        }

        public async Task<WorkingHours> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.WorkingHours.FirstAsync(w => w.Id == id);
        }

        public async Task<IReadOnlyList<WorkingHours>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.WorkingHours.ToListAsync();
        }

        public async Task<IReadOnlyList<WorkingHours>> ListAsync(Expression<Func<WorkingHours, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.WorkingHours.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<WorkingHours> UpdateAsync(WorkingHours entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry<WorkingHours>(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
