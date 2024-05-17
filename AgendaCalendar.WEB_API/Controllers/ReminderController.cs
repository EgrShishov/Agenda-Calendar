using AgendaCalendar.Application.Reminders.Commands;
using AgendaCalendar.WEB_API.Contracts.Reminders;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.WEB_API.Controllers
{
    [Route("api/reminder")]
    public class ReminderController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ReminderController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReminder(CreateReminderRequest request, int eventId)
        {
            var command = _mapper.Map<AddReminderCommand>((request, eventId));
            var createReminderResult = await _mediator.Send(command);

            return createReminderResult.Match(
                reminder => Ok(_mapper.Map<ReminderResponse>(reminder)),
                errors => Problem(errors));
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var deleteReminderResult = await _mediator.Send(new DeleteReminderCommand(id));
            return deleteReminderResult.Match(
                reminder => Ok(_mapper.Map<ReminderResponse>(reminder)),
                errors => Problem(errors));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditReminder(EditReminderRequest request, int id)
        {
            var command = _mapper.Map<UpdateReminderCommand>((request, id));
            var editReminderResult = await _mediator.Send(command);

            return editReminderResult.Match(
                reminder => Ok(_mapper.Map<ReminderResponse>(reminder)),
                errors => Problem(errors));
        }
    }
}
