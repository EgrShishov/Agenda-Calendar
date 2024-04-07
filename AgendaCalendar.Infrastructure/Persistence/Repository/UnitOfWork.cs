using AgendaCalendar.Infrastructure.Persistence.Data;

namespace AgendaCalendar.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _dbContext;
        private Lazy<IRepository<Calendar>> _calendarRepository;
        private Lazy<IRepository<Event>> _eventRepository;
        private Lazy<IRepository<Reminder>> _reminderRepository;
        private Lazy<IRepository<User>> _userRepository;
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            _calendarRepository = new(() => new CalendarRepository(_dbContext));
            _eventRepository = new(() => new EventsRepository(_dbContext));
            _reminderRepository = new(() => new ReminderRepository(_dbContext));
            _userRepository = new(() => new UsersRepository(_dbContext));
        }
        public IRepository<Calendar> CalendarRepository => _calendarRepository.Value;

        public IRepository<Event> EventRepository => _eventRepository.Value;

        public IRepository<Reminder> ReminderRepository => _reminderRepository.Value;

        public IRepository<User> UserRepository => _userRepository.Value;

        public async Task CreateDataBaseAsync()
        {
            await _dbContext.CreateDatabase();
        }

        public async Task DeleteDataBaseAsync()
        {
            await _dbContext.DeleteDatabase();
        }

        public async Task SaveAllAsync()
        {
            await _dbContext.SaveAllChanges();
        }
    }
}
