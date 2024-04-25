using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_APP.ViewModels.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.WEB_APP.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IMediator mediator, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager=signInManager;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if(user.)
        }
    }
}
