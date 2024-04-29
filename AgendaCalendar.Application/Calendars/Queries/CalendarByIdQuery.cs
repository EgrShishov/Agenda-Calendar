using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Queries
{
    public sealed record CalendarByIdQuery(int calendarId) : IRequest<ErrorOr<Calendar>> { }

    public class CalendarByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CalendarByIdQuery, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(CalendarByIdQuery request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);

            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }

            return calendar;
        }
    }
}
