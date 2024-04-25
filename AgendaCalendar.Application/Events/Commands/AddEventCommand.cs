
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record AddEventCommand(
        int CalednarId,
        int AuthorId,
        string Title,
        string Description,
        string Location,
        DateTime StartTime,
        DateTime EndTiem,
        RecurrenceRule RecurrenceRule
        ) : IRequest<Calendar> { }
    public class AddEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddEventCommand, Calendar>
    {
        public async Task<Calendar> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.CalednarId);
            if (calendar == null) return null;

            var newEvent = new Event
            {
                AuthorId = request.AuthorId,
                CalendarId = request.CalednarId,
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartTime = request.StartTime,
                EndTime = request.EndTiem,
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
