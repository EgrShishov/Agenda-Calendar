using System.ComponentModel.DataAnnotations;

namespace AgendaCalendar.WEB_APP.ViewModels.Calendar
{
    public class CreateCalendarViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Не указан электронный адрес")]
        [EmailAddress(ErrorMessage = "Некорректный электронный адрес")]
        public string OwnerEmail { get; set; }
    }
}
