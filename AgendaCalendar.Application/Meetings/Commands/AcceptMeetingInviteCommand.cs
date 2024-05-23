using AgendaCalendar.Application.Common.Interfaces;

namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record AcceptMeetingInviteCommand(int meetingId, int userId) : IRequest<ErrorOr<Unit>> { }

    public class AcceptMeetingInviteCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender) : IRequestHandler<AcceptMeetingInviteCommand, ErrorOr<Unit>>
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

            string subject = $"Meeting Accepted: {meeting.Title}";
            string body = $@"
            Dear friend,

            I am pleased to inform you that I have accepted the invitation for the following meeting:

            Title: {meeting.Title}
            Description: {meeting.Description}
            Start Time: {meeting.StartTime.ToString("dddd, MMMM d, yyyy h:mm tt")}
            End Time: {meeting.EndTime.ToString("dddd, MMMM d, yyyy h:mm tt")}

            I am looking forward to discussing the agenda and contributing to a productive discussion. Please let me know if there is anything else I need to prepare for the meeting.

            Thank you for the invitation, and I'll see you then.

            Best regards,
            {user.UserName}
            ";
            await emailSender.SendMessageAsync(user.Email, subject, body);

            await unitOfWork.InvitationRepository.UpdateAsync(invitation);
            await unitOfWork.SaveAllAsync();

            return Unit.Value;
        }
    }
}
