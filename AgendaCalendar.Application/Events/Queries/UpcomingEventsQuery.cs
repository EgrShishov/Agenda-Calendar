
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record UpcomingEventsQuery(int userId, int amount) : IRequest<ErrorOr<List<Event>>> { }

    public class UpcomingEventsQueryHandlet(IUnitOfWork unitOfWork) : IRequestHandler<UpcomingEventsQuery, ErrorOr<List<Event>>>
    {
        public async Task<ErrorOr<List<Event>>> Handle(UpcomingEventsQuery request, CancellationToken cancellationToken)
        {
            var date = DateTime.Now;

            var calendars = await unitOfWork.CalendarRepository.ListAsync(c => c.AuthorId == request.userId);
            if (calendars == null)
            {
                return Errors.Calendar.NotFound;
            }

            List<Event> upcomingEvents = new List<Event>();

            foreach(var calendar in calendars)
            {
                upcomingEvents = upcomingEvents.Concat(calendar.Events).ToList();
            }
            return upcomingEvents
                .Where(e => e.StartTime >= date)
                .OrderBy(e => e.StartTime)
                .Take(request.amount)
                .ToList();
        }
    }
}
