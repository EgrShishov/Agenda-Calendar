using AgendaCalendar.Domain.Entities;

namespace AgendaCalendar.WEB_APP.ViewModels.Calendar
{
    public class CalendarViewModel
    {
        public Domain.Entities.Calendar Calendar { get; set; }
        public User User { get; set; }
    }
}
