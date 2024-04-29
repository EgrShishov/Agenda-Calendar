using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record DeleteCalendarCommand(int calendarId) : IRequest<ErrorOr<Calendar>> { }

    public class DeleteCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }
            await unitOfWork.CalendarRepository.DeleteAsync(calendar.Id);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }

}
