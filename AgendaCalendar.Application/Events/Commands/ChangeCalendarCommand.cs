
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record ChangeCalendarCommand(int oldCalendarId, int newCalendarId, int eventId) : IRequest<Event> { }

    public class ChangeCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeCalendarCommand, Event>
    {
        public async Task<Event> Handle(ChangeCalendarCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null) return null;
            var oldCalendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.oldCalendarId);
            oldCalendar.Events.Remove(@event);
            var newCalendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.newCalendarId);
            newCalendar.Events.Add(@event);

            await unitOfWork.CalendarRepository.UpdateAsync(oldCalendar);
            await unitOfWork.CalendarRepository.UpdateAsync(newCalendar);
            await unitOfWork.SaveAllAsync();

            return @event;
        }
    }
}
