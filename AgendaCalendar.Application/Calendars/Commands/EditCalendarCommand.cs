
namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record EditCalendarCommand(string title,
            string description,
            int calendarId
            ) : IRequest<Calendar>
    { }
    public class EditCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<EditCalendarCommand, Calendar>
    {
        public async Task<Calendar> Handle(EditCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar is not null)
            {
                calendar.Title = request.title;
                calendar.CalendarDescription = request.description;
            }
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }
}
