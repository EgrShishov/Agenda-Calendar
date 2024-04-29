
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventByIdQuery(int eventId) : IRequest<ErrorOr<Event>> { }

    public class EventByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventByIdQuery, ErrorOr<Event>>
    {
        public async Task<ErrorOr<Event>> Handle(EventByIdQuery request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null)
            {
                return Errors.Event.NotFound;
            }
            return @event;
        }
    }
}
