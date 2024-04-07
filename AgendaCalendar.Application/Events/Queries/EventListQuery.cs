
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventListQuery(int calendarId) : IRequest<IReadOnlyList<Event>> { }

    public class EventListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventListQuery, IReadOnlyList<Event>>
    {
        public async Task<IReadOnlyList<Event>> Handle(EventListQuery request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null) return null;
            return calendar.Events;
        }
    }
}
