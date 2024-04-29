
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record AddEventParticipantCommand(int eventId, EventParticipant eventParticipant) : IRequest<ErrorOr<Event>> { }

    public class AddEventParticipantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddEventParticipantCommand, ErrorOr<Event>>
    {
        public async Task<ErrorOr<Event>> Handle(AddEventParticipantCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null)
            {
                return Errors.Event.NotFound;
            }
            
            @event.AddParticipant(request.eventParticipant);
            await unitOfWork.EventRepository.UpdateAsync(@event);
            await unitOfWork.SaveAllAsync();
            return @event;
        }
    }
}
