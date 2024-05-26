
namespace AgendaCalendar.Domain.Entities
{
    public class WorkingHours : Entity
    {
        public int UserId { get; set; }
        public DayOfWeek Day { get; set; }
        public List<DailyWorkingHours> DailyHours { get; set; } = new();
    }

    public class DailyWorkingHours : Entity
    {
        public int WorkingHoursId { get; set; }
        public WorkingHours WorkingHours { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
    }
}
