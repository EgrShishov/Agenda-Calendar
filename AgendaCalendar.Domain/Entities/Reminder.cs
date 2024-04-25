
namespace AgendaCalendar.Domain.Entities
{
    public class Reminder : Entity
    {
        public string Description { get; set; }

        public DateTime ReminderTime { get; set; }

        public string Email {  get; set; }

        public int EventId {  get; set; }

        public TimeSpan NotificationInterval { get; set; } = TimeSpan.FromHours(1); //as default - 1hour, but user can customize it
    }
}