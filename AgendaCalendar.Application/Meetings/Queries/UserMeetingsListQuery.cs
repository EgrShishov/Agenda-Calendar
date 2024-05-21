
namespace AgendaCalendar.Application.Meetings.Queries
{
    public sealed record UserMeetingsListQuery(int userId) : IRequest<ErrorOr<List<Meeting>>> { }

    public class UserMeetingsListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UserMeetingsListQuery, ErrorOr<List<Meeting>>>
    {
        public async Task<ErrorOr<List<Meeting>>> Handle(UserMeetingsListQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if(user == null)
            {
                return Errors.User.NotFound;
            }

            List<Meeting> user_meetings = new List<Meeting>();

            var meetings = await unitOfWork.MeetingRepository.ListAsync(m => m.UserId == request.userId);

            user_meetings = meetings.ToList();
            return user_meetings;
        }
    }
}
