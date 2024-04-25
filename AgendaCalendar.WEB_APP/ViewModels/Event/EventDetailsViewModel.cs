
namespace AgendaCalendar.WEB_APP.ViewModels.Event
{
    public class EventDetailsViewModel
    {
        public string Id { get; set; }
        public string Calendar { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(1);
        public string Location { get; set; }
        public string EventColor { get; set; }
    }
}
