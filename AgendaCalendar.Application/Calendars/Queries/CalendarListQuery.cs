
namespace AgendaCalendar.Application.Calendars.Queries
{
    public sealed record CalendarListQuery(int userId) : IRequest<ErrorOr<List<Calendar>>> { }

    public class CalendarListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CalendarListQuery, ErrorOr<List<Calendar>>>
    {
        public async Task<ErrorOr<List<Calendar>>> Handle(CalendarListQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if(user == null)
            {
                return Errors.User.NotFound;
            }

            var calendars = await unitOfWork.CalendarRepository.ListAsync(calendar => calendar.AuthorId == user.Id);
            return calendars.ToList();
        }
    }
}
