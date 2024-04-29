
namespace AgendaCalendar.Application.Events.Queries
{
    public sealed record EventParticipantsQuery(int eventId) : IRequest<ErrorOr<List<EventParticipant>>> { }

    public class EventParticipantQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EventParticipantsQuery, ErrorOr<List<EventParticipant>>>
    {
        public async Task<ErrorOr<List<EventParticipant>>> Handle(EventParticipantsQuery request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);

            if (@event == null)
            {
                return Errors.Event.NotFound;
            }
            return @event.EventParticipants;        
        }
    }
}
