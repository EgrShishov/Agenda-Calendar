using AgendaCalendar.Application.Meetings.Commands;
using AgendaCalendar.Application.Meetings.Queries;
using AgendaCalendar.Application.WorkingHours.Commands;
using AgendaCalendar.Application.WorkingHours.Queries;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_API.Contracts.Meetings;
using AgendaCalendar.WEB_API.Contracts.WorkingHours;
using AgendaCalendar.WEB_API.Extensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgendaCalendar.Application.Slots.Queries;
using AgendaCalendar.Application.Slots.Commands;
using AgendaCalendar.WEB_API.Contracts.Slots;
using AgendaCalendar.Domain.Common.Errors;

namespace AgendaCalendar.WEB_API.Controllers
{
    [Route("api/meeting")]
    [Authorize]
    public class MeetingController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MeetingController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateMeetingRequest request)
        {
            int userId = User.GetUserId();

            var command = _mapper.Map<AddMeetingCommand>((request, userId));

            var createMeetingResult = await _mediator.Send(command);

            return createMeetingResult.Match(
                meeting => Ok(_mapper.Map<MeetingResponse>(meeting)),
                errors => Problem(errors));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditMeetingRequest request, int meetingId)
        {
            var command = _mapper.Map<UpdateMeetingCommand>((request, meetingId));

            var createMeetingResult = await _mediator.Send(command);

            return createMeetingResult.Match(
                meeting => Ok(_mapper.Map<MeetingResponse>(meeting)),
                errors => Problem(errors));
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(int meetingId)
        {
            int userId = User.GetUserId();

            var command = new DeleteMeetingCommand(meetingId);

            var deleteMeetingResult = await _mediator.Send(command);

            return deleteMeetingResult.Match(
                meeting => Ok(_mapper.Map<MeetingResponse>(meeting)),
                errors => Problem(errors));
        }

        [HttpGet("invitations")]
        public async Task<IActionResult> GetUserInvitations()
        {
            int userId = User.GetUserId();

            var result = await _mediator.Send(new UserInvitationsQuery(userId));
            return result.Match(
                invitations => Ok(invitations),
                errors => Problem(errors));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUserMeetings()
        {
            int userId = User.GetUserId();

            var result = await _mediator.Send(new UserMeetingsListQuery(userId));

            return result.Match(
                meetings => Ok(_mapper.Map<List<MeetingResponse>>(meetings)),
                errors => Problem(errors));
        }

        [HttpPost("invite")]
        public async Task<IActionResult> InviteUserToMeeting(int meetingId, int userId)
        {
            var command = new InviteUserToMeetingCommand(meetingId, userId);
            var inviteResult = await _mediator.Send(command);

            return inviteResult.Match(
                invite => Ok(_mapper.Map<Meeting>(invite)),
                errors => Problem(errors));
        }

        [HttpGet("accept")]
        public async Task<IActionResult> AcceptMeeting(int meetingId)
        {
            int userId = User.GetUserId();

            var command = new AcceptMeetingInviteCommand(meetingId, userId);
            var acceptResult = await _mediator.Send(command);

            return acceptResult.Match(
                accept => Ok(_mapper.Map<Meeting>(accept)),
                errors => Problem(errors));
        }

        [HttpGet("decline")]
        public async Task<IActionResult> DeclineMeeting(int meetingId)
        {
            int userId = User.GetUserId();

            var command = new DeclineMeetingInviteCommand(meetingId, userId);
            var declineResult = await _mediator.Send(command);

            return declineResult.Match(
                decline => Ok(_mapper.Map<Meeting>(decline)),
                errors => Problem(errors));
        }

        [HttpGet("working_hours")]
        public async Task<IActionResult> GetWorkingHours(int userId)
        {
            var command = new GetUserWorkingHoursQuery(userId);
            var workingHoursResult = await _mediator.Send(command);

            return workingHoursResult.Match(
                hours => Ok(_mapper.Map<WorkingHoursResponse>(hours)),
                errors => Problem(errors));
        }

        [HttpPost("working_hours/set")]
        public async Task<IActionResult> SetWorkingHours(SetWorkingHoursRequest request)
        {
            int userId = User.GetUserId();

            var command = _mapper.Map<SetWorkingHoursCommand>((request, userId));
            var setWorkingHoursResult = await _mediator.Send(command);

            if (setWorkingHoursResult.IsError)
            {
                return Problem(setWorkingHoursResult.Errors);
            }

            var generateSlotsCommand = new GenerateSlotsCommand(userId);
            var generateSlotsResult = await _mediator.Send(generateSlotsCommand);

            return generateSlotsResult.Match(
                slots => Ok(),
                errors => Problem(errors));
        }
    }
}
