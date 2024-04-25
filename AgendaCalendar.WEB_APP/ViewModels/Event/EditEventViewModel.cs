using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgendaCalendar.WEB_APP.ViewModels.Event
{
    public class EditEventViewModel
    {
        public string Id { get; set; }
        public string SelectedCalendarId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(1);
        public string Location { get; set; }
        public IEnumerable<SelectListItem> UserCalendars { get; set; }
    }
}
