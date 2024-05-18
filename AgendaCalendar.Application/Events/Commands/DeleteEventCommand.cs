
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record DeleteEventCommand(int eventId) : IRequest<ErrorOr<Event>> { }

    public class DeleteEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEventCommand, ErrorOr<Event>>
    {
        public async Task<ErrorOr<Event>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null)
            {
                return Errors.Event.NotFound;
            }
            var calendar = await unitOfWork.CalendarRepository.ListAsync(c => c.Events.Contains(@event));
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }
            calendar[0].Events.Remove(@event);

            var reminders = await unitOfWork.ReminderRepository.ListAsync(r => r.EventId == @event.Id);

            if (reminders.Any())
            {
                var event_reminder = reminders.First();
                await unitOfWork.ReminderRepository.DeleteAsync(event_reminder.Id);
            }

            await unitOfWork.CalendarRepository.UpdateAsync(calendar[0]);
            await unitOfWork.EventRepository.DeleteAsync(request.eventId);
            await unitOfWork.SaveAllAsync();
            return @event;
        }
    }
}
