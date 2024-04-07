using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AgendaCalendar.API.ViewModels.Event
{
    public class CreateEventViewModel
    {
        public int SelectedCalendarId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(1);
        public string Location { get; set; }
        [Required]
        public IEnumerable<SelectListItem> UserCalendars { get; set; }
    }
}
