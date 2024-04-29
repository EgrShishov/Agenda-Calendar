using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record SubscribeToCalendarCommand(int userId, int calendarId) : IRequest<ErrorOr<Calendar>> { }

    public class SubscribeToCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SubscribeToCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(SubscribeToCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }

            if(user == null)
            {
                return Errors.User.NotFound;
            }

            calendar.SubscribersId.Add(user.Id);
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();

            return calendar;
        }
    }
}
