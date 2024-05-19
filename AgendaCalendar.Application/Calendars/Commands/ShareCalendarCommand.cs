using System.Security.Cryptography;
using System.Text;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record ShareCalendarCommand(int calendarId, int userId) : IRequest<ErrorOr<string>> { }

    public class ShareCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ShareCalendarCommand, ErrorOr<string>>
    {
        public async Task<ErrorOr<string>> Handle(ShareCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            
            if(calendar == null)
            {
                return Errors.Calendar.NotFound;
            }
            using var sha256 = SHA256.Create();

            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(calendar.ToString()));
            var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return $"https://localhost:7273/share/{request.calendarId}/{hash}";
        }
    }
}
