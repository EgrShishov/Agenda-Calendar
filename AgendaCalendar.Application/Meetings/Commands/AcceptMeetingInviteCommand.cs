
namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record AcceptMeetingInviteCommand(int meetingId, int userId) : IRequest<ErrorOr<Unit>> { }

    public class AcceptMeetingInviteCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AcceptMeetingInviteCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(AcceptMeetingInviteCommand request, CancellationToken cancellationToken)
        {
            var meeting = await unitOfWork.MeetingRepository.GetByIdAsync(request.meetingId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if (meeting == null)
            {
                return Errors.Meetings.NotFound;
            }

            if (user == null)
            {
                return Errors.User.NotFound;
            }

            var invitations = await unitOfWork.InvitationRepository.ListAsync(i => i.MeetingId == request.meetingId && i.UserId == request.userId);
            if (invitations.First() == null)
            {
                return Errors.Meetings.InvitationNotFound;
            }
            var invitation = invitations.First();

            invitation.Status = InvitationStatus.Accepted;

            //create event?

            /*            var meeting_event = new Event
                        {
                            Title = $"Meeting with {meeting.Description}",
                            StartTime = meeting.StartTime,
                            EndTime = meeting.EndTime,
                            Description = meeting.Description, //add main calendar_property?
                            AuthorId = meeting.UserId,
                        };*/

            await unitOfWork.InvitationRepository.UpdateAsync(invitation);
            await unitOfWork.SaveAllAsync();

            return Unit.Value;
        }
    }
}
