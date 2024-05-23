using AgendaCalendar.Application.Common.Interfaces;

namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record InviteUserToMeetingCommand(int meetingId, int userId) : IRequest<ErrorOr<Meeting>> { }

    public class InviteUserToMeetingCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender) 
        : IRequestHandler<InviteUserToMeetingCommand, ErrorOr<Meeting>>
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

            //tokens?
            string yourDomain = "https://localhost:7127";
            string subject = $"Meeting scheduled: {meeting.Title} ({user.UserName}) - {meeting.StartTime} - {meeting.EndTime}";
            string body = $"Dear {user.UserName}, I hope this email finds you well. " +
                $"I would like to invite you to a meeting with the following details:" +
                $"\r\n\r\nTitle: {meeting.Title}\r\n" +
                $"Description: {meeting.Description}\r\n" +
                $"Start Time: {meeting.StartTime.ToString("dddd, MMMM d, yyyy h:mm tt")}\r\n" +
                $"End Time: {meeting.EndTime.ToString("dddd, MMMM d, yyyy h:mm tt")}" +
                $"<a href=\"\"{yourDomain}/api/meetings/accept?meetingId={{meeting.Id}}\"\">Accept Meeting</a>" +
                $"<a href=\"\"{yourDomain}/api/meetings/decline?meetingId={{meeting.Id}}\"\">Decline Meeting</a>";
            await emailSender.SendMessageAsync(user.Email, subject, body); 

            await unitOfWork.InvitationRepository.AddAsync(invitation);
            await unitOfWork.SaveAllAsync();

            return meeting;
        }
    }

}
