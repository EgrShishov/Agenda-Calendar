using _De_SerializationLib;
using System.Text;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record ImportCalendarCommand(byte[] calendar_bytes, int author_id) : IRequest<Calendar> { }

    public class ImportCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ImportCalendarCommand, Calendar>
    {
        public async Task<Calendar> Handle(ImportCalendarCommand request, CancellationToken cancellationToken)
        {
            var ical_format = Encoding.UTF8.GetString(request.calendar_bytes);
            Calendar calendar = IcalConverter.Deserialize(ical_format);
            if (calendar == null) throw new Exception("Error in deserializing calendar");
            Console.WriteLine(calendar.Title);
            calendar.AuthorId = request.author_id;
            await unitOfWork.CalendarRepository.AddAsync(calendar);
            await unitOfWork.SaveAllAsync();
            //added version comparing, if author and name and uid are equal(also in deserialization lib)
            return calendar;
        }
    }
}
