
namespace AgendaCalendar.Application.Reminders.Commands
{
    public sealed record UpdateReminderCommand(
        int ReminderId,
        int EventId,
        string Description,
        string Email,
        DateTime ReminderTime,
        TimeSpan NotificationInterval
        ) : IRequest<ErrorOr<Reminder>> { }

    public class EditReminderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateReminderCommand, ErrorOr<Reminder>>
    {
        public async Task<ErrorOr<Reminder>> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await unitOfWork.ReminderRepository.GetByIdAsync(request.ReminderId);
            if (reminder == null)
            {
                return Errors.Reminder.NotFound;
            }

            reminder.Description = request.Description;
            reminder.ReminderTime = request.ReminderTime;
            reminder.NotificationInterval = request.NotificationInterval;
            reminder.EventId = request.EventId;
            reminder.Email = request.Email;

            await unitOfWork.ReminderRepository.UpdateAsync(reminder);
            await unitOfWork.SaveAllAsync();
            return reminder;
        }
    }
}
