using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    { 
        private readonly IMediator _medaitor;

        public HomeController(IMediator mediator) 
        {
            _medaitor = mediator;
        }
    }
}
