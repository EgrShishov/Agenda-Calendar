using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record EditCalendarCommand(string title,
            string description,
            int calendarId
            ) : IRequest<ErrorOr<Calendar>>
    { }
    public class EditCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<EditCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(EditCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar is not null)
            {
                calendar.Title = request.title;
                calendar.CalendarDescription = request.description;
            }
            else 
            {
                return Errors.Calendar.NotFound;
            }
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }
}
