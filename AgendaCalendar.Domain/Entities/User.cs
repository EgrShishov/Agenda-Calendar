using Microsoft.AspNetCore.Identity;

namespace AgendaCalendar.Domain.Entities
{
    public class User : Entity// IdentityUser<int>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime BirthdayDate { get; set; }
        public override string ToString()
        {
            return $"User: {UserName}, password: {Password}, email: {Email}";
        }
    }
}