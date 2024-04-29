
namespace AgendaCalendar.Application.Reminders.Commands
{
    public sealed record DeleteReminderCommand(int reminderId) : IRequest<ErrorOr<Reminder>> { }

    public class DeleteReminderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReminderCommand, ErrorOr<Reminder>>
    {
        public async Task<ErrorOr<Reminder>> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await unitOfWork.ReminderRepository.GetByIdAsync(request.reminderId);
            if(reminder is null)
            {
                return Errors.Reminder.NotFound;
            }

            await unitOfWork.ReminderRepository.DeleteAsync(reminder.Id);
            await unitOfWork.SaveAllAsync();

            return reminder;
        }
    }
}
