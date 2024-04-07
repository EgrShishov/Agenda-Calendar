
namespace AgendaCalendar.Application.Calendars.Queries
{
    public sealed record CalendarListQuery(int userId) : IRequest<IReadOnlyList<Calendar>> { }

    public class CalendarListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CalendarListQuery, IReadOnlyList<Calendar>>
    {
        public async Task<IReadOnlyList<Calendar>> Handle(CalendarListQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if(user != null)
            {
                var calendars = await unitOfWork.CalendarRepository.ListAsync(calendar => calendar.AuthorId == user.Id);
                return calendars;
            }
            return null;
        }
    }
}
