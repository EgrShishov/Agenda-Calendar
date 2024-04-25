
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventByIdQuery(int eventId) : IRequest<Event> { }

    public class EventByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventByIdQuery, Event>
    {
        public async Task<Event> Handle(EventByIdQuery request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null) return null;
            return @event;
        }
    }
}
