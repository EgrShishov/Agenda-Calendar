
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record UpdateEventCommand(Event @event) : IRequest<Event> { }

    public class UpdateEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateEventCommand, Event>
    {
        public async Task<Event> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.EventRepository.UpdateAsync(request.@event);
            var calendars = await unitOfWork.CalendarRepository.ListAsync(x => x.Events.Contains(request.@event));

            var calendar = calendars.First();
            var index = calendar.Events.FindIndex(x => x.Id.Equals(request.@event.Id));
            if (index != -1)
                calendar.Events[index] = request.@event;

            var updatedEvent = await unitOfWork.EventRepository.UpdateAsync(request.@event);
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            return updatedEvent;
        }
    }
}
