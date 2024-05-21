
namespace AgendaCalendar.Domain.Entities
{
    public class WorkingHours : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public DayOfWeek Day { get; set; }
        public List<DailyWorkingHours> DailyHours { get; set; } 
    }

    public class DailyWorkingHours
    {
        public int WorkingHoursId { get; set; }

        public WorkingHours WorkingHours { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }


}
