
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record ChangeCalendarCommand(int oldCalendarId, int newCalendarId, int eventId) : IRequest<ErrorOr<Event>> { }

    public class ChangeCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeCalendarCommand, ErrorOr<Event>>
    {
        public async Task<ErrorOr<Event>> Handle(ChangeCalendarCommand request, CancellationToken cancellationToken)
        {
            var @event = await unitOfWork.EventRepository.GetByIdAsync(request.eventId);
            if (@event == null)
            {
                return Errors.Event.NotFound;
            }
            var oldCalendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.oldCalendarId);
            if(oldCalendar == null)
            {
                return Errors.Calendar.NotFound;
            }
            oldCalendar.Events.Remove(@event);
            var newCalendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.newCalendarId);
            if(newCalendar == null)
            {
                return Errors.Calendar.NotFound;
            }
            newCalendar.Events.Add(@event);

            await unitOfWork.CalendarRepository.UpdateAsync(oldCalendar);
            await unitOfWork.CalendarRepository.UpdateAsync(newCalendar);
            await unitOfWork.SaveAllAsync();

            return @event;
        }
    }
}
