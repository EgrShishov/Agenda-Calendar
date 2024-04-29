
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventListQuery(int calendarId) : IRequest<ErrorOr<List<Event>>> { }

    public class EventListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventListQuery, ErrorOr<List<Event>>>
    {
        public async Task<ErrorOr<List<Event>>> Handle(EventListQuery request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }    
            return calendar.Events;
        }
    }
}
