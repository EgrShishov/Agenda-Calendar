using System.ComponentModel.DataAnnotations;

namespace AgendaCalendar.WEB_APP.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
