
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record AddEventCommand(
        int CalednarId,
        int AuthorId,
        string Title,
        string Description,
        string Location,
        DateTime StartTime,
        DateTime EndTime,
        RecurrenceRule RecurrenceRule
        ) : IRequest<ErrorOr<Calendar>> { }
    public class AddEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddEventCommand, ErrorOr<Calendar>>
    {
        public async Task<ErrorOr<Calendar>> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.CalednarId);
            if (calendar == null)
            {
                return Errors.Calendar.NotFound;
            }

            var newEvent = new Event
            {
                AuthorId = request.AuthorId,
                CalendarId = request.CalednarId,
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ReccurenceRules = request.RecurrenceRule,
                EventParticipants = new(),
            };
            calendar.Events.Add(newEvent);
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.EventRepository.AddAsync(newEvent);
            await unitOfWork.SaveAllAsync();
            return calendar;
        }
    }
}
