
namespace AgendaCalendar.Application.Meetings.Commands
{
    public sealed record DeleteMeetingCommand(int meetingId) : IRequest<ErrorOr<Meeting>> { }

    public class DeleteMeetingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMeetingCommand, ErrorOr<Meeting>>
    {

        public async Task<ErrorOr<Meeting>> Handle(DeleteMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await unitOfWork.MeetingRepository.GetByIdAsync(request.meetingId);
            if (meeting == null)
            {
                return Errors.Meetings.NotFound;
            }

            await unitOfWork.MeetingRepository.DeleteAsync(meeting.Id);
            await unitOfWork.SaveAllAsync();

            return meeting;
        }
    }

}
