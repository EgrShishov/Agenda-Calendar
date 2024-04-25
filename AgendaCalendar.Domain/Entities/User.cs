using Microsoft.AspNetCore.Identity;

namespace AgendaCalendar.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime BirthdayDate { get; set; }
    }
}