using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.API.ViewModels.Calendar
{
    public class CalendarViewModel
    {
        public Domain.Entities.Calendar Calendar { get; set; }
        public User User { get; set; }
    }
}
