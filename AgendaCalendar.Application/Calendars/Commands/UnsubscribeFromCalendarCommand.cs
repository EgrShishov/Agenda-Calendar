
namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record UnsubscribeFromCalendarCommand(int userId, int calendarId) : IRequest<Calendar> { }

    public class UnsubscribeFromCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UnsubscribeFromCalendarCommand, Calendar>
    {
        public async Task<Calendar> Handle(UnsubscribeFromCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (calendar == null || user == null) return null;

            calendar.SubscribersId.Remove(user.Id);
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();
            
            return calendar;
        }
    }
}
