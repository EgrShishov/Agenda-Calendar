using _De_SerializationLib;
using System.Text;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record ExportCalendarCommand(int calendarId) : IRequest<ErrorOr<byte[]>> { }

    public class ExportCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ExportCalendarCommand, ErrorOr<byte[]>>
    {
        public async Task<ErrorOr<byte[]>> Handle(ExportCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }
            var serialized_calendar = IcalConverter.Serialize(calendar);
            if (serialized_calendar == null)
            {
                return Errors.Calendar.SerializationError;
            }

            byte[] buffer = Encoding.Default.GetBytes(serialized_calendar);
            return buffer;
        }
    }
}
