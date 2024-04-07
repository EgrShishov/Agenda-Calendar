
namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record UpdateCalendarCommand(int calendarId, 
        string description, 
        string title
        ) : IRequest<Calendar> { }

    public class UpdateCalendarCommandHandlet(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCalendarCommand, Calendar>
    {
        public async Task<Calendar> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null) return null;
            var newCalendar = new Calendar()
            {
                Title = request.title,
                CalendarDescription = request.description,
                Events = calendar.Events ?? new List<Event>(),
                Reminders = calendar.Reminders ?? new List<Reminder>(),
                Subscribers = calendar.Subscribers ?? new List<int>(),
            };
            calendar = newCalendar;
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }
}
