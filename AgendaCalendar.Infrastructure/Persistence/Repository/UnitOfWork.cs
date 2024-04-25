using AgendaCalendar.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _dbContext;
        private UserManager<User> _userManager;
        private Lazy<IRepository<Calendar>> _calendarRepository;
        private Lazy<IRepository<Event>> _eventRepository;
        private Lazy<IRepository<Reminder>> _reminderRepository;
        private Lazy<IUserRepository> _userRepository;

        public UnitOfWork(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;

            _calendarRepository = new(() => new CalendarRepository(_dbContext));
            _eventRepository = new(() => new EventsRepository(_dbContext));
            _reminderRepository = new(() => new ReminderRepository(_dbContext));
            _userRepository = new(() => new UsersRepository(_dbContext, _userManager));
        }
        public IRepository<Calendar> CalendarRepository => _calendarRepository.Value;

        public IRepository<Event> EventRepository => _eventRepository.Value;

        public IRepository<Reminder> ReminderRepository => _reminderRepository.Value;

        public IUserRepository UserRepository => _userRepository.Value;

        public async Task CreateDataBaseAsync() => await _dbContext.Database.EnsureCreatedAsync();
        public async Task DeleteDataBaseAsync() => await _dbContext.Database.EnsureDeletedAsync();
        public async Task SaveAllAsync() => await _dbContext.SaveChangesAsync();
    }
}
