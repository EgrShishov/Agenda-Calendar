using AgendaCalendar.Application.Common.Interfaces;

namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record DeclineMeetingInviteCommand(int meetingId, int userId) : IRequest<ErrorOr<Unit>> { }

    public class DeclineMeetingInviteCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender) : IRequestHandler<DeclineMeetingInviteCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(DeclineMeetingInviteCommand request, CancellationToken cancellationToken)
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

            invitation.Status = InvitationStatus.Declined;

            string subject = $"Meeting Declined: {meeting.Title}";
            string body = $@"
                Dear friend,

                I regret to inform you that I am unable to attend the following meeting:

                Title: {meeting.Title}
                Description: {meeting.Description}
                Start Time: {meeting.StartTime.ToString("dddd, MMMM d, yyyy h:mm tt")}
                End Time: {meeting.EndTime.ToString("dddd, MMMM d, yyyy h:mm tt")}

                Unfortunately, I have a scheduling conflict and will not be able to participate. I apologize for the inconvenience and hope you are able to find an alternative time that works for the other attendees.

                Please let me know if there is anything else I can do to assist with the planning of this meeting.

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
