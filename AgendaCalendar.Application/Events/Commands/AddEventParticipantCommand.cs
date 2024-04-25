
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record AddEventParticipantCommand(int eventId, EventParticipant eventParticipant) : IRequest<Event> { }

    public class AddEventParticipantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddEventParticipantCommand, Event>
    {
        public async Task<Event> Handle(AddEventParticipantCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null) return null;
            
            @event.AddParticipant(request.eventParticipant);
            await unitOfWork.EventRepository.UpdateAsync(@event);
            await unitOfWork.SaveAllAsync();
            return @event;
        }
    }
}
