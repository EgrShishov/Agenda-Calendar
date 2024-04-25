using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.WEB_API.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    { 
        private readonly IMediator _medaitor;

        public HomeController(IMediator mediator) 
        {
            _medaitor = mediator;
        }
    }
}
