using AgendaCalendar.Application.Events.Commands;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Application.Reminders.Commands;
using AgendaCalendar.WEB_API.Contracts.Events;
using AgendaCalendar.WEB_API.Extensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.WEB_API.Controllers
{
    [Route("api/events")]
    public class EventController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public EventController(IMediator mediator, IMapper mapper) 
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateEventRequest request, int calendarId)
        {
            int authorId = User.GetUserId();
            string authorEmail = User.GetEmail();

            var command = _mapper.Map<AddEventCommand>((request, calendarId, authorId));

            var createEventResult = await _mediator.Send(command);

            var createReminderResult = await _mediator.Send(
                new AddReminderCommand(
                    request.Description,
                    request.StartTime,
                    authorEmail,
                    createEventResult.Value.Id,
                    TimeSpan.Zero
            ));

            return createEventResult.Match(
                created => Ok(_mapper.Map<EventResponse>(created)),
                errors => Problem(errors));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditEventRequest request, int eventId, int calendarId)
        {
            int authorId = User.GetUserId();
            var command = _mapper.Map<UpdateEventCommand>((request, eventId, authorId, calendarId));

            var editEventResult = await _mediator.Send(command);

            return editEventResult.Match(
                newEvent => Ok(_mapper.Map<EventResponse>(newEvent)),
                errors => Problem(errors));
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteEventResult = await _mediator.Send(new DeleteEventCommand(id));

            return deleteEventResult.Match(
                deletedEvent => Ok(_mapper.Map<EventResponse>(deletedEvent)),
                errors => Problem(errors));
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> UpcomingEvents()
        {
            int userId = User.GetUserId();
            int amount = 5;

            var upcomingEventsResult = await _mediator.Send(new UpcomingEventsQuery(userId, amount));

            return upcomingEventsResult.Match(
                upcoming => Ok(_mapper.Map<List<EventResponse>>(upcoming)),
                errors => Problem(errors));
        }

    }
}
