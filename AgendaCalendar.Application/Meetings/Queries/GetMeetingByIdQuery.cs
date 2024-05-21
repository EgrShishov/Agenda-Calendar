
namespace AgendaCalendar.Application.Meetings.Queries
{
    public sealed record GetMeetingByIdQuery(int meetingId) : IRequest<ErrorOr<Meeting>> { }

    public class GetMeetingByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMeetingByIdQuery, ErrorOr<Meeting>>
    {
        public async Task<ErrorOr<Meeting>> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
        {
            var meeting = await unitOfWork.MeetingRepository.GetByIdAsync(request.meetingId);

            if(meeting == null)
            {
                return Errors.Meetings.NotFound;
            }

            return meeting;
        }
    }
}
