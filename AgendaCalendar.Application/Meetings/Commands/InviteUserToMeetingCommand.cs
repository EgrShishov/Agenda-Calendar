
namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record InviteUserToMeetingCommand(int meetingId, int userId) : IRequest<ErrorOr<Meeting>> { }

    public class InviteUserToMeetingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<InviteUserToMeetingCommand, ErrorOr<Meeting>>
    {
        public async Task<ErrorOr<Meeting>> Handle(InviteUserToMeetingCommand request, CancellationToken cancellationToken)
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

            var existingInvitations = await unitOfWork.InvitationRepository
                .ListAsync(i => i.MeetingId == request.meetingId && i.UserId == request.userId);

            Invitation existingInvitation = null;
            if (existingInvitations.Any())
            {
                existingInvitation = existingInvitations.First();
            }

            if (existingInvitation != null)
            {
                return Errors.Meetings.AlreadyInvited;
            }

            var invitation = new Invitation
            {
                MeetingId = request.meetingId,
                UserId = request.userId,
                Status = InvitationStatus.Pending
            };

            await unitOfWork.InvitationRepository.AddAsync(invitation);
            await unitOfWork.SaveAllAsync();

            return meeting;
        }
    }

}
