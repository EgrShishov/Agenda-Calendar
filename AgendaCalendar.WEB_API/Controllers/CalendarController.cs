using _De_SerializationLib;
using AgendaCalendar.Application.Calendars.Commands;
using AgendaCalendar.Application.Calendars.Queries;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Calendars;
using AgendaCalendar.WEB_API.Extensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AgendaCalendar.WEB_API.Controllers
{
    [Route("api/calendar")]
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<Event>>> GetCalendars()
        {
            int userId = User.GetUserId();
            var userCalendarsResult = await _mediator.Send(new CalendarListQuery(userId));

            List<Event> events = new List<Event>();
            List<Event> upcomingEvents = new List<Event>();
            string icalEvents = string.Empty;
            foreach (var calendar in userCalendarsResult.Value)
            {
                events = events.Concat(calendar.Events).ToList();
                var calendarsUpcomingResult = await _mediator.Send(new EventListByDateQuery(calendar.Id, DateTime.Now));
                upcomingEvents = upcomingEvents.Concat(calendarsUpcomingResult.Value).ToList();
                icalEvents += JsonConverter.GetJsonEventList(calendar.CalendarColor, calendar.Events);
            }

            if (icalEvents.StartsWith("["))
            {
                icalEvents = icalEvents.Substring(1);
            }
            if (icalEvents.EndsWith("]"))
            {
                icalEvents = icalEvents.Substring(0, icalEvents.Length - 1);
            }
            string[] jsonArrays = icalEvents.Split(new string[] { "][" }, StringSplitOptions.RemoveEmptyEntries);

            icalEvents = "[" + string.Join(",", jsonArrays) + "]";
            
            return Ok(icalEvents);
        }

        [HttpGet("calendars")]
        public async Task<IActionResult> Calendars()
        {
            int userId = User.GetUserId();
            var userCalendarsResult = await _mediator.Send(new CalendarListQuery(userId));
            return userCalendarsResult.Match(
                userCalendarResult => Ok(_mapper.Map<List<CalendarResponse>>(userCalendarResult)),
                errors => Problem(errors));
        } 

        [HttpGet("export")]
        public async Task<IActionResult> Export(int id) 
        {
            var exportCalendarCommandResult = await _mediator.Send(new ExportCalendarCommand(id));

            var calendarByIdResult = await _mediator.Send(new CalendarByIdQuery(id));

            return exportCalendarCommandResult.Match(
                bytes =>
                {
                    var stream = new MemoryStream(bytes);

                    Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{calendarByIdResult.Value.Title}.ics\"");
                    Response.ContentType = "application/calendar";

                    return File(stream, "application/calendar");
                },
                errors => Problem(errors));
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            int id = User.GetUserId();

            string filename = Path.GetFileName(file.FileName);

            using MemoryStream mstream = new();
            await file.CopyToAsync(mstream);
            byte[] calendar_bytes = mstream.ToArray();

            var calendarImportResult = await _mediator.Send(new ImportCalendarCommand(calendar_bytes, id));

            return calendarImportResult.Match(
                calendarImportResult => Ok(_mapper.Map<CalendarResponse>(calendarImportResult)),
                errors => Problem(errors));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCalendarRequest request)
        {
            int id = User.GetUserId();

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
                _ => Ok(_mapper.Map<CalendarResponse>(calendarDeleteResult)),
                errors => Problem(errors));
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(int id)
        {
            int userId = User.GetUserId();

            var subscribeCalendarResult = await _mediator.Send(new SubscribeToCalendarCommand(userId, id));
            return subscribeCalendarResult.Match(
                subscribeCalendarResult => Ok(_mapper.Map<CalendarResponse>(subscribeCalendarResult)),
                errors => Problem(errors));
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe(int id)
        {
            int userId = User.GetUserId();

            var subscribeCalendarResult = await _mediator.Send(new UnsubscribeFromCalendarCommand(userId, id));
            return subscribeCalendarResult.Match(
                subscribeCalendarResult => Ok(_mapper.Map<CalendarResponse>(subscribeCalendarResult)),
                errors => Problem(errors));
        }

    }
}
