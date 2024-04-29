
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record DeleteEventParticipantCommand(int eventId, EventParticipant eventParticipant) : IRequest<ErrorOr<Event>> { }

    public class DeleteEventParticipantCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEventParticipantCommand, ErrorOr<Event>>
    {
        public async Task<ErrorOr<Event>> Handle(DeleteEventParticipantCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null)
            {
                return Errors.Event.NotFound;
            }

            @event.RemoveParticipant(request.eventParticipant);
            await unitOfWork.EventRepository.UpdateAsync(@event);
            await unitOfWork.SaveAllAsync();
            return @event;
        }
    }
}
