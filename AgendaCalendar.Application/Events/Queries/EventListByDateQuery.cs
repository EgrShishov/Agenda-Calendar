
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventListByDateQuery(int calendarId, DateTime date) : IRequest<IEnumerable<Event>> { }

    public class EventListByDateQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventListByDateQuery, IEnumerable<Event>>
    {
        public async Task<IEnumerable<Event>> Handle(EventListByDateQuery request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar is not null)
                return calendar.Events.Where(x => x.StartTime.Date == request.date.Date);
            return null;
        }
    }
}
