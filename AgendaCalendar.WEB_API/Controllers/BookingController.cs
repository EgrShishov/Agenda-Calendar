using AgendaCalendar.Application.Slots.Commands;
using AgendaCalendar.Application.Slots.Queries;
using AgendaCalendar.WEB_API.Contracts.Meetings;
using AgendaCalendar.WEB_API.Contracts.Slots;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.WEB_API.Controllers
{
    [Route("api/booking")]
    public class BookingController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BookingController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvaibleSlots(string email)
        {
            var command = new GetAvaibaleSlotsQuery(email);
            var avaibleSlotsResult = await _mediator.Send(command);

            return avaibleSlotsResult.Match(
                response => Ok(_mapper.Map<ScheduleResponse>(response)),
                errors => Problem(errors));
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookSlot(BookSlotRequest requst, int slotId)
        {
            var command = _mapper.Map<BookSlotCommand>((requst, slotId));
            var bookSlotResult = await _mediator.Send(command);

            return bookSlotResult.Match(
                meeting => Ok(_mapper.Map<MeetingResponse>(meeting)),
                errors => Problem(errors));
        }
    }
}
