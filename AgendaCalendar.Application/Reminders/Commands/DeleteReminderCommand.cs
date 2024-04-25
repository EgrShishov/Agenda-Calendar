
namespace AgendaCalendar.Application.Reminders.Commands
{
    public sealed record DeleteReminderCommand(int reminderId) : IRequest<Reminder> { }

    public class DeleteReminderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReminderCommand, Reminder>
    {
        public async Task<Reminder> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await unitOfWork.ReminderRepository.GetByIdAsync(request.reminderId);

            await unitOfWork.ReminderRepository.DeleteAsync(reminder.Id);
            await unitOfWork.SaveAllAsync();

            return reminder;
        }
    }
}
