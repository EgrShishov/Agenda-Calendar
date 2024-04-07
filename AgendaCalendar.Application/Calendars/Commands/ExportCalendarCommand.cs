using _De_SerializationLib;
using System.Text;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record ExportCalendarCommand(int calendarId) : IRequest<byte[]> { }

    public class ExportCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ExportCalendarCommand, byte[]>
    {
        public async Task<byte[]> Handle(ExportCalendarCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.calendarId);
            if (calendar == null) return null;
            
            var serialized_calendar = IcalConverter.Serialize(calendar);
            if (serialized_calendar == null) return null;

            byte[] buffer = Encoding.Default.GetBytes(serialized_calendar);
            return buffer;
        }
    }
}
