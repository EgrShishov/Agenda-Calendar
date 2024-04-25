
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventListByDateRangeQuery(int calendarId, DateTime startDate, DateTime endDate) : IRequest<IEnumerable<Event>> { }

    public class EventListByDateRangeQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventListByDateRangeQuery, IEnumerable<Event>>
    {
        public async Task<IEnumerable<Event>> Handle(EventListByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar is not null)
                return calendar.Events.Where(x => x.StartTime.Date >= request.startDate.Date && x.EndTime.Date <= request.endDate.Date);
            return null;
        }
    }
}
