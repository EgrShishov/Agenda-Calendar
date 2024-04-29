using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record AddCalendarByUrlCommand(string url): IRequest<ErrorOr<Calendar>> { }

    public class AddCalendarByUrlCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddCalendarByUrlCommand, ErrorOr<Calendar>>
    {
        public Task<ErrorOr<Calendar>> Handle(AddCalendarByUrlCommand request, CancellationToken cancellationToken)
        {
            using HttpClient client = new();
            throw new NotImplementedException();
        }
      }
}
