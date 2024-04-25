
namespace AgendaCalendar.Application.Events.Commands
{
    public sealed record UpdateEventCommand(
        int EventId,
        int AuthorId,
        int CalendarId,
        string Title,
        string Description,
        string Location,
        DateTime StartTime,
        DateTime EndTime,
        RecurrenceRule RecurrenceRule
        ) : IRequest<Event> { }

    public class UpdateEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateEventCommand, Event>
    {
        public async Task<Event> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.CalendarId);
            if (calendar is null) return null;
            var existingEvent = calendar.Events.FirstOrDefault(e => e.Id == request.EventId);

            if (existingEvent is not null)
            {
                existingEvent.Title = request.Title;
                existingEvent.Description = request.Description;
                existingEvent.Location = request.Location;
                existingEvent.StartTime = request.StartTime;
                existingEvent.EndTime = request.EndTime;
                existingEvent.ReccurenceRules = request.RecurrenceRule;
            }
            else
            {
                var newEvent = new Event
                {
                    Title = request.Title,
                    Description = request.Description,
                    Location = request.Location,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    ReccurenceRules = request.RecurrenceRule,
                    AuthorId = request.AuthorId
                };
                calendar.Events.Add(newEvent);
            }
            await unitOfWork.CalendarRepository.UpdateAsync(calendar);
            await unitOfWork.SaveAllAsync();

            return existingEvent;
        }
    }
}
