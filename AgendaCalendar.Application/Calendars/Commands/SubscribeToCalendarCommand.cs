using System.Security.Cryptography;
using System.Text;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record SubscribeToCalendarCommand(int userId, string calendarUrl) : IRequest<ErrorOr<Calendar>> { }

    public class SubscribeToCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SubscribeToCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(SubscribeToCalendarCommand request, CancellationToken cancellationToken)
        {
            int calendarId = -1;

            if (!string.IsNullOrEmpty(request.calendarUrl))
            {
                var parts = request.calendarUrl.Split('/');
                if (parts.Length < 2 || !int.TryParse(parts[^2], out calendarId))
                {
                    return Errors.Calendar.InvalidUrl;
                }
            }
            else
            {
                return Errors.Calendar.NotFound;
            }

            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(calendarId);
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.userId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }

            if (user == null)
            {
                return Errors.User.NotFound;
            }

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(calendar.ToString()));
            var expectedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            if (!string.IsNullOrEmpty(request.calendarUrl))
            {
                if (!request.calendarUrl.EndsWith(expectedHash))
                {
                    return Errors.Calendar.InvalidUrl;
                }
            }

            if (calendar.AuthorId == request.userId)
            {
                return Errors.Calendar.SubscriberIsAuthor;
            }


            if (calendar.SubscribersId.Contains(request.userId))
            {
                return Errors.Calendar.AlreadySubscribed;
            }

            calendar.SubscribersId.Add(user.Id);
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();

            return calendar;
        }
    }
}
