
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventListByDateRangeQuery(int calendarId, DateTime startDate, DateTime endDate) : IRequest<ErrorOr<List<Event>>> { }

    public class EventListByDateRangeQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventListByDateRangeQuery, ErrorOr<List<Event>>>
    {
        public async Task<ErrorOr<List<Event>>> Handle(EventListByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar is null)
            {
                return Errors.Calendar.NotFound;
            }
            return calendar.Events.Where(x => x.StartTime.Date >= request.startDate.Date && x.EndTime.Date <= request.endDate.Date).ToList();
        }
    }
}
