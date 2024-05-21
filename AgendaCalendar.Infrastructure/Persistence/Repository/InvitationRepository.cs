using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class InvitationRepository : IRepository<Invitation>
    {
        private readonly AppDbContext _dbContext;
        public InvitationRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<Invitation> AddAsync(Invitation entity, CancellationToken cancellationToken = default)
        {
            var invitation = await _dbContext.Invitations.AddAsync(entity);
            return invitation.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var invitation = await _dbContext.Invitations.FirstOrDefaultAsync(i => i.Id == id);
            _dbContext.Entry<Invitation>(invitation).State = EntityState.Modified;
        }

        public async Task<Invitation> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Invitations.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IReadOnlyList<Invitation>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Invitations.ToListAsync();
        }

        public async Task<IReadOnlyList<Invitation>> ListAsync(Expression<Func<Invitation, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Invitations.AsQueryable();

            if(filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<Invitation> UpdateAsync(Invitation entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
