using _De_SerializationLib;
using AgendaCalendar.Application.Calendars.Commands;
using AgendaCalendar.Application.Calendars.Queries;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Application.Reminders.Commands;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Calendars;
using AgendaCalendar.WEB_API.Contracts.Reminders;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace AgendaCalendar.WEB_API.Controllers
{
    [Route("api/calendar")]
    public class CalendarController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CalendarController(IMediator mediator, IMapper mapper) 
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("u")]
        public async Task<ActionResult<IEnumerable<Event>>> GetCalendars(int userId)
        {
            var userCalendars = await _mediator.Send(new CalendarListQuery(userId));
            if(userCalendars is null)
            {
                return NotFound();
            }
            List<Event> events = new List<Event>();
            List<Event> upcomingEvents = new List<Event>();
            foreach (var calendar in userCalendars)
            {
                events = events.Concat(calendar.Events).ToList();
                var calendarsUpcoming = await _mediator.Send(new EventListByDateQuery(calendar.Id, DateTime.Now));
                upcomingEvents = upcomingEvents.Concat(calendarsUpcoming).ToList();
            }
            var icalEvents = JsonConverter.GetJsonEventList(events);
            return Ok(icalEvents);
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export(int id) 
        {
            var bytes = await _mediator.Send(new ExportCalendarCommand(id));
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));
            return File(bytes, "text/plain", $"{calendar.Title}.ics");
        }

        [HttpPost("import")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file, int id)
        {
            string filename = Path.GetFileName(file.FileName);
            using MemoryStream mstream = new();
            await file.CopyToAsync(mstream);
            byte[] calendar_bytes = mstream.ToArray();
            var calendar = await _mediator.Send(new ImportCalendarCommand(calendar_bytes, id));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCalendarRequest request, int id)
        {
            var command = _mapper.Map<CreateCalendarCommand>((request, id));
            var createCalendarResult = await _mediator.Send(command);

            return Ok(_mapper.Map<CalendarResponse>(createCalendarResult));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditCalendarRequest request, int id)
        {
            var command = _mapper.Map<EditCalendarCommand>((request, id));
            var editCalendarResult = await _mediator.Send(command);

            return Ok(_mapper.Map<CalendarResponse>(editCalendarResult));
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));
            if (calendar == null) return NotFound();
            calendar = await _mediator.Send(new DeleteCalendarCommand(id));
            if (calendar is null) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe(int id)
        {
            throw new NotImplementedException();
        }

/*        [HttpPost("create")]
        public async Task<IActionResult> CreateReminder(CreateReminderRequest request, int id)
        {
            var command = _mapper.Map<AddReminderCommand>((request, id));
            var createReminderResult = await _mediator.Send(command);

            return Ok(_mapper.Map<ReminderResponse>(createReminderResult));
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var reminder = await _mediator.Send(new DeleteReminderCommand(id));
            if (reminder == null) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditReminder(EditReminderRequest request, int id)
        {
            var command = _mapper.Map<UpdateReminderCommand>((request, id));
            var editReminderResult = await _mediator.Send(command);

            return Ok(_mapper.Map<ReminderResponse>(editReminderResult));
        }*/

    }
}
