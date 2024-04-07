
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record DeleteEventCommand(int eventId) : IRequest<Event> { }

    public class DeleteEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEventCommand, Event>
    {
        public async Task<Event> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null) return null;
            var calendar = await unitOfWork.CalendarRepository.ListAsync(c => c.Events.Contains(@event));
            if (calendar == null) return null;
            calendar[0].Events.Remove(@event);

            await unitOfWork.CalendarRepository.UpdateAsync(calendar[0]);
            await unitOfWork.EventRepository.DeleteAsync(request.eventId);
            await unitOfWork.SaveAllAsync();
            return @event;
        }
    }
}
