
namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record SubscribeToCalendarCommand(int userId, int calendarId) : IRequest<Calendar> { }

    public class SubscribeToCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SubscribeToCalendarCommand, Calendar>
    {
        public async Task<Calendar> Handle(SubscribeToCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (calendar == null || user == null) return null;

            calendar.SubscribersId.Add(user.Id);
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();

            return calendar;
        }
    }
}
