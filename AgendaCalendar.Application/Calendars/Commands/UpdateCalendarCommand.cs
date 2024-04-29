using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record UpdateCalendarCommand(int calendarId, 
        string description, 
        string title
        ) : IRequest<ErrorOr<Calendar>> { }

    public class UpdateCalendarCommandHandlet(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }

            calendar.Title = request.title;
            calendar.CalendarDescription = request.description;

            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }
}
