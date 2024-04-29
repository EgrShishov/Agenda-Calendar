using ErrorOr;

namespace AgendaCalendar.Application.Calendars.Commands
{
    public sealed record CreateCalendarCommand(string title, 
        string description,
        int authorId) : IRequest<ErrorOr<Calendar>>
    { }
    public class CreateCalendarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCalendarCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(CreateCalendarCommand request, CancellationToken cancellationToken)
        {
            var new_calendar = new Calendar()
            {
                Title = request.title, 
                CalendarDescription = request.description, 
                AuthorId = request.authorId, 
                Events = new List<Event>(), 
                Reminders = new List<Reminder>() 
            };
            await unitOfWork.CalendarRepository.AddAsync(new_calendar);
            await unitOfWork.SaveAllAsync();
            return new_calendar;
        }
    }
}
