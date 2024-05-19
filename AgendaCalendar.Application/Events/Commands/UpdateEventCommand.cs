
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
        ) : IRequest<ErrorOr<Event>> { }

    public class UpdateEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateEventCommand, ErrorOr<Event>>
    {
        public async Task<ErrorOr<Event>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var calendar = await unitOfWork.CalendarRepository.GetByIdAsync(request.CalendarId);
            if (calendar is null)
            {
                return Errors.Calendar.NotFound;
            }

            var author = await unitOfWork.UserRepository.GetByIdAsync(request.AuthorId);
            if (author is null)
            {
                return Errors.User.NotFound;
            }

            var existingEvent = calendar.Events.FirstOrDefault(e => e.Id == request.EventId);

            if (existingEvent is not null)
            {
                var now = DateTime.Now;
                if (existingEvent.StartTime != request.StartTime)
                {
                    var event_reminder = await unitOfWork.ReminderRepository.ListAsync(r => r.EventId == existingEvent.Id);
                    if (event_reminder.Any())
                    {
                        var reminder = event_reminder.First();
                        if(request.StartTime < now)
                        {
                            await unitOfWork.ReminderRepository.DeleteAsync(reminder.Id);
                        }
                        else
                        {
                            reminder.ReminderTime = request.StartTime;
                            await unitOfWork.ReminderRepository.UpdateAsync(reminder);
                        }
                    } 
                    else if (request.StartTime > now)
                    {
                        Reminder reminder = new Reminder
                        {
                            ReminderTime = request.StartTime,
                            EventId = existingEvent.Id,
                            Description = existingEvent.Description,
                            Email = author.Email,
                            NotificationInterval = TimeSpan.Zero
                        };

                        await unitOfWork.ReminderRepository.AddAsync(reminder);
                    }
                }
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
