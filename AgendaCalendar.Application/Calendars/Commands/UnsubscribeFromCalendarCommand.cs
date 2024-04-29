using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record UnsubscribeFromCalendarCommand(int userId, int calendarId) : IRequest<ErrorOr<Calendar>> { }

    public class UnsubscribeFromCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UnsubscribeFromCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(UnsubscribeFromCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }

            if (user == null)
            {
                return Errors.User.NotFound;
            }

            calendar.SubscribersId.Remove(user.Id);
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            
            return calendar;
        }
    }
}
