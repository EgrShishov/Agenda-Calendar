
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record DeleteEventParticipantCommand(int eventId, EventParticipant eventParticipant) : IRequest<Event> { }

    public class DeleteEventParticipantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEventParticipantCommand, Event>
    {
        public async Task<Event> Handle(DeleteEventParticipantCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null) return null;

            @event.RemoveParticipant(request.eventParticipant);
            await unitOfWork.EventRepository.UpdateAsync(@event);
            await unitOfWork.SaveAllAsync();
            return @event;
        }
    }
}
