
namespace AgendaCalendar.Application.Reminders.Commands
{
    public sealed record AddReminderCommand(string desc, DateTime time, string emailAdress, int eventId, TimeSpan interval)
        : IRequest<ErrorOr<Reminder>> { }

    public class AddReminderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddReminderCommand, ErrorOr<Reminder>>
    {
        public async Task<ErrorOr<Reminder>> Handle(AddReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = new Reminder()
            {
                Description = request.desc,
                ReminderTime = request.time,
                Email = request.emailAdress,
                EventId = request.eventId,
                NotificationInterval = request.interval
            };
            await unitOfWork.ReminderRepository.AddAsync(reminder);
            await unitOfWork.SaveAllAsync();

            return reminder;
        }
    }
}
