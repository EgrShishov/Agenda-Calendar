
namespace AgendaCalendar.Application.Reminders.Commands
{
    public sealed record UpdateReminderCommand(
        int ReminderId,
        int EventId,
        string Description,
        string Email,
        DateTime ReminderTime,
        TimeSpan NotificationInterval
        ) : IRequest<Reminder> { }

    public class EditReminderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateReminderCommand, Reminder>
    {
        public async Task<Reminder> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await unitOfWork.ReminderRepository.GetByIdAsync(request.ReminderId);
            if (reminder == null) return null;

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
