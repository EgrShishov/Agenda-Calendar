using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.API.Controllers
{
    [Route("Calendar/[controller]")]
    [ApiController]
    public class SettingsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
