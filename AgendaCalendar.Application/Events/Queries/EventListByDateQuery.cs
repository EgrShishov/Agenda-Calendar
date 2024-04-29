
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventListByDateQuery(int calendarId, DateTime date) : IRequest<ErrorOr<List<Event>>> { }

    public class EventListByDateQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventListByDateQuery, ErrorOr<List<Event>>>
    {
        public async Task<ErrorOr<List<Event>>> Handle(EventListByDateQuery request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar is null)
            {
                return Errors.Calendar.NotFound;
            }
            return calendar.Events.Where(x => x.StartTime.Date == request.date.Date).ToList();
        }
    }
}
