
namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record UpdateMeetingCommand(
        int meetingId,
        string title, 
        DateTime startTime,
        DateTime endTime,
        string description) : IRequest<ErrorOr<Meeting>> { }

    public class UpdateMeetingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateMeetingCommand, ErrorOr<Meeting>>
    {
        public async Task<ErrorOr<Meeting>> Handle(UpdateMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await unitOfWork.MeetingRepository.GetByIdAsync(request.meetingId);
            if (meeting == null)
            {
                return Errors.Meetings.NotFound;
            }

            meeting.Title = request.title;
            meeting.StartTime = request.startTime;
            meeting.EndTime = request.endTime;
            meeting.Description = request.description;

            await unitOfWork.MeetingRepository.UpdateAsync(meeting);
            await unitOfWork.SaveAllAsync();

            return meeting;
        }
    }

}
