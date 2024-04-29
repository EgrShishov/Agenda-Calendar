using _De_SerializationLib;
using AgendaCalendar.Application.Calendars.Commands;
using AgendaCalendar.Application.Calendars.Queries;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Calendars;
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

            List<Event> events = new List<Event>();
            List<Event> upcomingEvents = new List<Event>();
            foreach (var calendar in userCalendars.Value)
            {
                events = events.Concat(calendar.Events).ToList();
                var calendarsUpcomingResult = await _mediator.Send(new EventListByDateQuery(calendar.Id, DateTime.Now));
                upcomingEvents = upcomingEvents.Concat(calendarsUpcomingResult.Value).ToList();
            }
            var icalEvents = JsonConverter.GetJsonEventList(events);
            return Ok(icalEvents);
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export(int id) 
        {
            var bytes = await _mediator.Send(new ExportCalendarCommand(id));

            var calendar = await _mediator.Send(new CalendarByIdQuery(id));

            return bytes.Match(
                bytes => File(bytes, "text/plain", $"{calendar.Value.Title}.ics"),
                errors => Problem(errors));
        }

        [HttpPost("import")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file, int id)
        {
            string filename = Path.GetFileName(file.FileName);

            using MemoryStream mstream = new();
            await file.CopyToAsync(mstream);
            byte[] calendar_bytes = mstream.ToArray();

            var calendarImportResult = await _mediator.Send(new ImportCalendarCommand(calendar_bytes, id));

            return calendarImportResult.Match(
                calendarImportResult => RedirectToAction(nameof(Index)),
                errors => Problem(errors));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCalendarRequest request, int id)
        {
            var command = _mapper.Map<CreateCalendarCommand>((request, id));

            var createCalendarResult = await _mediator.Send(command);

            return createCalendarResult.Match(
                crateCalendarResult => Ok(_mapper.Map<CalendarResponse>(createCalendarResult)),
                errors => Problem(errors));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditCalendarRequest request, int id)
        {
            var command = _mapper.Map<EditCalendarCommand>((request, id));

            var editCalendarResult = await _mediator.Send(command);

            return editCalendarResult.Match(
                editCalendarResult => Ok(_mapper.Map<CalendarResponse>(editCalendarResult)),
                errors => Problem(errors));
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));

            var calendarDeleteResult = await _mediator.Send(new DeleteCalendarCommand(id));

            return calendarDeleteResult.Match(
                _ => RedirectToAction(nameof(Index)),
                errors => Problem(errors));
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

    }
}
