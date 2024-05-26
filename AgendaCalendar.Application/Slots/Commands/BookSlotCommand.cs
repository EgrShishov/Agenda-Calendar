using AgendaCalendar.Application.Common.Interfaces;

namespace AgendaCalendar.Application.Slots.Commands
{
    public sealed record BookSlotCommand(
        string firstName,
        string lastName,
        string email,
        string description,
        int slotId) : IRequest<ErrorOr<Meeting>> { }

    public class BookSlotCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender) : IRequestHandler<BookSlotCommand, ErrorOr<Meeting>>
    {
        public async Task<ErrorOr<Meeting>> Handle(BookSlotCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetUserByEmailAsync(request.email);
            if (user == null)
            {
                return Errors.User.NotFound;
            }

            var slot = await unitOfWork.SlotRepository.GetByIdAsync(request.slotId);
            if (slot == null) 
            {
                return Errors.Slot.NotFound;
            }

            if (slot.IsBooked)
            {
                return Errors.Slot.AlreadyBooked;
            }

            slot.IsBooked = true;

            await unitOfWork.SlotRepository.UpdateAsync(slot);

            TimeOnly startTime;
            TimeOnly endTime;

            TimeOnly.TryParse(slot.Times.First(), out startTime);
            TimeOnly.TryParse(slot.Times.First(), out endTime);

            var meeting = new Meeting
            {
                Title = $"Meeting with {request.email}",
                Description = request.description,
                StartTime = slot.Date + startTime.ToTimeSpan(),
                EndTime = slot.Date + endTime.ToTimeSpan() + TimeSpan.FromHours(1),
                UserId = user.Id,
                Invitations = new List<Invitation>
                {
                    new Invitation
                    {
                        MeetingId = slot.Id,
                        UserId = user.Id,
                        Status = InvitationStatus.Pending
                    }
                }
            };

            string yourDomain = "https://localhost:7127";
            string subject = $"Meeting scheduled: {meeting.Title} ({user.UserName}) - {meeting.StartTime} - {meeting.EndTime}";
            string body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f6f6f6;
                            color: #333333;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            width: 100%;
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            border: 1px solid #dddddd;
                            padding: 20px;
                        }}
                        .header {{
                            background-color: #ff9800;
                            color: #ffffff;
                            padding: 10px 20px;
                            text-align: center;
                        }}
                        .content {{
                            margin: 20px 0;
                        }}
                        .content p {{
                            line-height: 1.6;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin: 10px 0;
                            text-align: center;
                            text-decoration: none;
                            color: #ffffff;
                            background-color: #ff9800;
                            border-radius: 5px;
                            border: 2px solid #ff9800;
                            font-weight: bold;
                        }}
                        .button:hover {{
                            background-color: #ffffff;
                            color: #ff9800;
                        }}
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            font-size: 12px;
                            color: #777777;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Meeting Invitation</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {user.UserName},</p>
                            <p>I would like to invite you to a meeting with the following details:</p>
                            <p><strong>Title:</strong> {meeting.Title}</p>
                            <p><strong>Description:</strong> {meeting.Description}</p>
                            <p><strong>Start Time:</strong> {meeting.StartTime.ToString("dddd, MMMM d, yyyy h:mm tt")}</p>
                            <p><strong>End Time:</strong> {meeting.EndTime.ToString("dddd, MMMM d, yyyy h:mm tt")}</p>
                            <p>
                                <form action='{yourDomain}/api/meetings/accept?meetingId={meeting.Id}' method='post' style='display:inline;'>
                                    <input type='hidden' name='meetingId' value='{meeting.Id}' />
                                    <button type='submit' class='button'>Accept Meeting</button>
                                </form>
                                <form action='{yourDomain}/api/meetings/decline?meetingId={meeting.Id}' method='post' style='display:inline;'>
                                    <input type='hidden' name='meetingId' value='{meeting.Id}' />
                                    <button type='submit' class='button'>Decline Meeting</button>
                                </form>
                            </p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 AgendaCalendar. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await emailSender.SendMessageAsync(user.Email, subject, body);

            var addedMeeting = await unitOfWork.MeetingRepository.AddAsync(meeting);

            await unitOfWork.SaveAllAsync();
            return addedMeeting;
        }
    }
}
