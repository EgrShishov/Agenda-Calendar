
namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record AddCalendarByUrlCommand(string url): IRequest<Calendar> { }

    public class AddCalendarByUrlCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddCalendarByUrlCommand, Calendar>
    {
        public Task<Calendar> Handle(AddCalendarByUrlCommand request, CancellationToken cancellationToken)
        {
            using HttpClient client = new();
            throw new NotImplementedException();
        }
      }
}
