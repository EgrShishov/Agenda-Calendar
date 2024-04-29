using AgendaCalendar.Domain.Common.Errors;
using ErrorOr;
using _De_SerializationLib;
using System.Text;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record ImportCalendarCommand(byte[] calendar_bytes, int author_id) : IRequest<ErrorOr<Calendar>> { }

    public class ImportCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ImportCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(ImportCalendarCommand request, CancellationToken cancellationToken)
        {
            var ical_format = Encoding.UTF8.GetString(request.calendar_bytes);
            Calendar calendar = IcalConverter.Deserialize(ical_format);
            if (calendar == null)
            {
                return Errors.Calendar.SerializationError;
            }

            var author = await unitOfWork.UserRepository.GetByIdAsync(request.author_id);
            if (author == null)
            {
                return Errors.User.NotFound;
            }

            calendar.AuthorId = request.author_id;
            await unitOfWork.CalendarRepository.AddAsync(calendar);
            await unitOfWork.SaveAllAsync();
            //added version comparing, if author and name and uid are equal(also in deserialization lib)
            return calendar;
        }
    }
}
