
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

            calendar.Title = request.title;
            calendar.CalendarDescription = request.description;

            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }
}
