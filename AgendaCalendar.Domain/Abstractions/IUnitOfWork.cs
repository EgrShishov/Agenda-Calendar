using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<Calendar> CalendarRepository { get; }
        IRepository<Event> EventRepository { get; }
        IRepository<Reminder> ReminderRepository { get; }
        IUserRepository UserRepository { get; }
        IRepository<Meeting> MeetingRepository { get; }
        IRepository<WorkingHours> WorkHoursRepository { get; }
        IRepository<Invitation> InvitationRepository { get; }

        public Task DeleteDataBaseAsync();
        public Task CreateDataBaseAsync();
        public Task SaveAllAsync();
    }
}
